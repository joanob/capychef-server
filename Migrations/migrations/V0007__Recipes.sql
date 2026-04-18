-- RECIPES
                                                    
CREATE TABLE recipes
(
    id                   SERIAL PRIMARY KEY,
    name                 VARCHAR(50) NOT NULL,
    description          TEXT,
    difficulty           INTEGER     NOT NULL,
    cooking_time_minutes INTEGER     NOT NULL,
    servings             INTEGER     NOT NULL,
    is_global            BOOLEAN     NOT NULL,
    global_id            VARCHAR,
    household_id         INTEGER,
    created_at           TIMESTAMP   NOT NULL,
    created_by           INTEGER,
    row_version          INTEGER     NOT NULL,
    is_deleted           BOOLEAN     NOT NULL,
    deleted_at           TIMESTAMP,
    FOREIGN KEY (household_id) REFERENCES households (id),
    FOREIGN KEY (created_by) REFERENCES users (id),
    UNIQUE (household_id, modified_global_supermarket_id),
    CHECK (
        (is_global IS TRUE AND global_id IS NOT NULL AND household_id IS NULL AND created_by IS NULL)
            OR
        (is_global IS FALSE AND global_id IS NULL AND household_id IS NOT NULL AND created_by IS NOT NULL)
        )
);

-- RECIPES TAGS
    
CREATE TABLE recipes_tags
(
    id          SERIAL PRIMARY KEY,
    recipe_id   INTEGER NOT NULL,
    tag      VARCHAR(50) NOT NULL,
    created_at   TIMESTAMP NOT NULL,
    created_by   INTEGER NOT NULL,
    FOREIGN KEY (recipe_id) REFERENCES recipes (id),
    FOREIGN KEY (created_by) REFERENCES users (id)
);

CREATE INDEX idx_recipes_tags_recipe_id ON recipes_tags (recipe_id);

-- RECIPES INGREDIENTS
  
CREATE TABLE recipes_ingredients
(
    id                   SERIAL PRIMARY KEY,
    recipe_id            INTEGER NOT NULL,
    food_id              INTEGER NOT NULL,
    quantity             REAL,
    food_uom_id          INTEGER,
    created_at           TIMESTAMP NOT NULL,
    created_by           INTEGER NOT NULL,
    FOREIGN KEY (recipe_id) REFERENCES recipes (id),
    FOREIGN KEY (food_id) REFERENCES food (id),
    FOREIGN KEY (food_uom_id) REFERENCES food_uom (id),
    FOREIGN KEY (created_by) REFERENCES users (id)
);

CREATE INDEX idx_recipes_ingredients_recipe_id ON recipes_ingredients (recipe_id);

-- RECIPES STEPS

CREATE TABLE recipes_steps
(
    id                   SERIAL PRIMARY KEY,
    recipe_id            INTEGER NOT NULL,
    step_number          INTEGER NOT NULL,
    description          TEXT NOT NULL,
    created_at           TIMESTAMP NOT NULL,
    created_by           INTEGER NOT NULL,
    FOREIGN KEY (recipe_id) REFERENCES recipes (id),
    FOREIGN KEY (created_by) REFERENCES users (id)
);

CREATE INDEX idx_recipes_steps_recipe_id ON recipes_steps (recipe_id);
