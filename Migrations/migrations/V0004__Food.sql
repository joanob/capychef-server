-- UNITS OF MEASURE
    
CREATE TABLE uom_dimensions (
  code VARCHAR(4) PRIMARY KEY,
  name TEXT NOT NULL  
);

CREATE RULE "uom_dimensions_soft_delete" AS ON DELETE TO "uom_dimensions" DO INSTEAD NOTHING;
                                                      
CREATE TABLE uom
(
    code                VARCHAR(4) PRIMARY KEY,
    name                TEXT       NOT NULL,
    dimension           VARCHAR(4) NOT NULL,
    base_uom VARCHAR(4),
    numerator           INTEGER,
    denominator         INTEGER,
    FOREIGN KEY (dimension) REFERENCES uom_dimensions (code),
    FOREIGN KEY (base_uom) REFERENCES uom (code) DEFERRABLE INITIALLY DEFERRED, -- Avoid foreign key errors on mass load
    CHECK (
        (base_uom IS NULL AND numerator IS NULL AND denominator IS NULL) OR
        (base_uom IS NOT NULL AND numerator IS NOT NULL AND denominator IS NOT NULL)
        )
);

CREATE RULE "uom_soft_delete" AS ON DELETE TO "uom" DO INSTEAD NOTHING;                                               

-- FOOD CATEGORIES

CREATE TABLE food_categories
(
    id             INTEGER PRIMARY KEY,
    name       TEXT      NOT NULL,
    is_leaf BOOLEAN NOT NULL,
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
    base_uom VARCHAR(4) NOT NULL,
    days_until_expiration INTEGER,
    days_until_best_before INTEGER,
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
    FOREIGN KEY (base_uom) REFERENCES uom (code),
    FOREIGN KEY (modified_global_food_id) REFERENCES food (id),
    FOREIGN KEY (created_by) REFERENCES users (id),
    UNIQUE (household_id, modified_global_food_id),
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

CREATE VIEW active_food AS
SELECT *
FROM food
WHERE is_deleted = FALSE;
            
-- FOOD MODIFICATIONS HISTORY
                                  
CREATE TABLE food_modifications_history
(
    id      SERIAL PRIMARY KEY,
    food_id INTEGER NOT NULL,
    column_name TEXT NOT NULL,
    previous_value TEXT,
    new_value TEXT,
    modified_at TIMESTAMP NOT NULL,
    modified_by INTEGER NOT NULL,
    FOREIGN KEY (food_id) REFERENCES food (id),
    FOREIGN KEY (modified_by) REFERENCES users (id)
);

CREATE INDEX idx_food_modifications_history_food_id ON food_modifications_history(food_id);

CREATE RULE "food_modifications_history_soft_deletion" AS ON DELETE TO "food_modifications_history" DO INSTEAD NOTHING;
                                                                    
-- FOOD UOM

CREATE TABLE food_uom
(
    id                SERIAL PRIMARY KEY,
    food_id INTEGER NOT NULL,
    uom varchar(4) NOT NULL,
    base_uom VARCHAR(4),
    numerator           INTEGER,
    denominator         INTEGER,
    FOREIGN KEY (food_id) REFERENCES food (id),
    FOREIGN KEY (base_uom) REFERENCES uom (code),
    UNIQUE (food_id, uom),
    CHECK (
        (base_uom IS NULL AND numerator IS NULL AND denominator IS NULL) OR
        (base_uom IS NOT NULL AND numerator IS NOT NULL AND denominator IS NOT NULL)
        )
);                                                                 