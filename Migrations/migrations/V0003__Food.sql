-- FOOD CATEGORIES

CREATE TABLE food_categories
(
    id             SERIAL PRIMARY KEY,
    name       TEXT      NOT NULL,
    parent_category_id INTEGER,
    FOREIGN KEY (parent_category_id) REFERENCES food_categories (id)
);

CREATE RULE "food_categories_soft_deletion" AS ON DELETE TO "food_categories" DO INSTEAD NOTHING;
          
-- GLOBAL FOOD
                                                    
CREATE TABLE global_food (
  id SERIAL PRIMARY KEY,
  name TEXT NOT NULL,
    category_id INTEGER NOT NULL,
  FOREIGN KEY (category_id) REFERENCES food_categories (id)
);

CREATE RULE "global_food_soft_deletion" AS ON DELETE TO "global_food" DO INSTEAD NOTHING;
                                                    
-- HOUSEHOLD FOOD                                                     
                                                         
CREATE TABLE household_food (
     id SERIAL PRIMARY KEY,
     created_at     TIMESTAMP NOT NULL,
     row_version    INTEGER   NOT NULL,
     is_deleted     BOOLEAN   NOT NULL,
     deleted_at     TIMESTAMP,
     created_by INTEGER NOT NULL,
     household_id INTEGER NOT NULL,
     modified_global_food_id INTEGER,
     name TEXT NOT NULL,
     category_id INTEGER NOT NULL,
     FOREIGN KEY (household_id) REFERENCES households (id),
     FOREIGN KEY (created_by) REFERENCES users (id),
     FOREIGN KEY (modified_global_food_id) REFERENCES global_food (id),
     FOREIGN KEY (category_id) REFERENCES food_categories (id)
);

CREATE RULE "household_food_soft_delete" AS ON DELETE TO "household_food" DO INSTEAD (
    UPDATE household_food
    SET is_deleted = true
    WHERE id = old.id
      AND NOT is_deleted
    );
                                                      
-- HOUSEHOLD FOOD MODIFICATIONS

