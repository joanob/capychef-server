-- RECIPES
                                                    
CREATE TABLE recipes
(
    id                   SERIAL PRIMARY KEY,
    recipe_type VARCHAR(1) NOT NULL,
    household_id         INTEGER,
    name                 VARCHAR(50) NOT NULL,
    description          TEXT,
    difficulty           INTEGER     NOT NULL,
    cooking_time_minutes INTEGER     NOT NULL,
    servings             INTEGER     NOT NULL,
    published_at         TIMESTAMP,
    published_by         INTEGER,
    household_recipe_id  INTEGER,
    reviewed_at          TIMESTAMP,
    reviewed_by          INTEGER,
    review_message       TEXT,
    created_at           TIMESTAMP   NOT NULL,
    created_by           INTEGER,
    row_version          INTEGER     NOT NULL,
    is_deleted           BOOLEAN     NOT NULL,
    deleted_at           TIMESTAMP,
    FOREIGN KEY (household_id) REFERENCES households (id),
    FOREIGN KEY (created_by) REFERENCES users (id),
    FOREIGN KEY (household_recipe_id) REFERENCES recipes (id),
    FOREIGN KEY (reviewed_by) REFERENCES users (id)
);

-- RECIPES TAGS
    
CREATE TABLE recipes_tags
(
    id          SERIAL PRIMARY KEY,
    recipe_id   INTEGER NOT NULL,
    order_num   INTEGER NOT NULL,
    tag         VARCHAR(50) NOT NULL,
    created_at  TIMESTAMP NOT NULL,
    created_by  INTEGER NOT NULL,
    FOREIGN KEY (recipe_id) REFERENCES recipes (id),
    FOREIGN KEY (created_by) REFERENCES users (id)
);

CREATE INDEX idx_recipes_tags_recipe_id ON recipes_tags (recipe_id);

-- RECIPES INGREDIENTS
  
CREATE TABLE recipes_ingredients
(
    id             SERIAL PRIMARY KEY,
    recipe_id      INTEGER NOT NULL,
    order_num      INTEGER NOT NULL,
    food_id        INTEGER NOT NULL,
    quantity       REAL,
    food_uom_id    INTEGER,
    alternative_to INTEGER,
    created_at     TIMESTAMP NOT NULL,
    created_by     INTEGER NOT NULL,
    FOREIGN KEY (recipe_id) REFERENCES recipes (id),
    FOREIGN KEY (food_id) REFERENCES food (id),
    FOREIGN KEY (food_uom_id) REFERENCES food_uom (id),
    FOREIGN KEY (alternative_to) REFERENCES recipes_ingredients (id),
    FOREIGN KEY (created_by) REFERENCES users (id)
);

CREATE INDEX idx_recipes_ingredients_recipe_id ON recipes_ingredients (recipe_id);

-- RECIPES STEPS

CREATE TABLE recipes_steps
(
    id          SERIAL PRIMARY KEY,
    recipe_id   INTEGER NOT NULL,
    step_number INTEGER NOT NULL,
    description TEXT    NOT NULL,
    created_at  TIMESTAMP NOT NULL,
    created_by  INTEGER NOT NULL,
    FOREIGN KEY (recipe_id) REFERENCES recipes (id),
    FOREIGN KEY (created_by) REFERENCES users (id)
);

CREATE INDEX idx_recipes_steps_recipe_id ON recipes_steps (recipe_id);

-- RECIPES HOUSEHOLD DETAILS 

CREATE TABLE recipes_household_details
(
    id          SERIAL PRIMARY KEY,
    recipe_id   INTEGER NOT NULL,
    household_id INTEGER NOT NULL,
    user_id      INTEGER,
    is_favourite BOOLEAN NOT NULL,
    score    INTEGER NOT NULL,
    min_days_between_consumptions INTEGER,
    max_days_between_consumptions INTEGER,
    created_at  TIMESTAMP NOT NULL,
    created_by  INTEGER NOT NULL,
    row_version          INTEGER     NOT NULL,
    is_deleted           BOOLEAN     NOT NULL,
    deleted_at           TIMESTAMP,
    FOREIGN KEY (recipe_id) REFERENCES recipes (id),
    FOREIGN KEY (household_id) REFERENCES households (id),
    FOREIGN KEY (user_id) REFERENCES users (id),
    FOREIGN KEY (created_by) REFERENCES users (id)
);