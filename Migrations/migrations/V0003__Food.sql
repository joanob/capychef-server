-- FOOD CATEGORIES

CREATE TABLE food_categories
(
    id             SERIAL PRIMARY KEY,
    name       TEXT      NOT NULL,
    parent_category_id INTEGER,
    FOREIGN KEY (parent_category_id) REFERENCES food_categories (id)
);

CREATE RULE "food_categories_soft_deletion" AS ON DELETE TO "food_categories" DO INSTEAD NOTHING;
          
-- FOOD
                                                    
CREATE TABLE food
(
    id                      SERIAL PRIMARY KEY,
    name                    TEXT      NOT NULL,
    category_id             INTEGER   NOT NULL,
    is_global               BOOLEAN   NOT NULL,
    global_id               VARCHAR,
    household_id            INTEGER,
    modified_global_food_id INTEGER,
    created_at              TIMESTAMP NOT NULL,
    created_by              INTEGER,
    row_version             INTEGER   NOT NULL,
    is_deleted              BOOLEAN   NOT NULL,
    deleted_at              TIMESTAMP,
    FOREIGN KEY (category_id) REFERENCES food_categories (id),
    FOREIGN KEY (modified_global_food_id) REFERENCES food (id),
    FOREIGN KEY (created_by) REFERENCES users (id),
    UNIQUE (household_id, modifies_global_id),
    CHECK (
        (is_global IS TRUE AND global_id IS NOT NULL AND household_id IS NULL AND modified_global_food_id IS NULL AND
         created_by IS NULL)
            OR
        (is_global IS FALSE AND global_id IS NULL AND household_id IS NOT NULL AND created_by IS NOT NULL)
        )
);

CREATE INDEX idx_food_household_id ON food(household_id);

CREATE RULE "food_soft_deletion" AS ON DELETE TO "food" DO INSTEAD (
    UPDATE food
    SET is_deleted = true
    WHERE id = old.id
      AND NOT is_deleted
    );
                                                     