-- RECIPES
-- Global recipes are created by Capychef and made available to all households. They cannot be edited or deleted by users.
-- Global recipes have is_global = true, is_public = true, household_recipe_id = null
-- Household recipes are created by users and are only visible within their household. They can be edited and deleted by household members.
-- Household recipes have is_global = false, is_public = false, household_recipe_id = null
-- Public recipes are household recipes that have been approved by Capychef and made available to all households. They can be edited and deleted by their creators, but not by other users.
-- Public recipes have is_global = false, is_public = true, household_recipe_id = id of the original household recipe
-- Draft recipes are household recipes that are pending validation
-- Draft recipes have is_global = false, is_public = false, household_recipe_id = id of the original household recipe. Reviewed or not depends on reviewed_at and reviewed_by fields.
                                                    
CREATE TABLE recipes
(
    id                   SERIAL PRIMARY KEY,
    name                 VARCHAR(50) NOT NULL,
    description          TEXT,
    difficulty           INTEGER     NOT NULL,
    cooking_time_minutes INTEGER     NOT NULL,
    servings             INTEGER     NOT NULL,
    is_global            BOOLEAN     NOT NULL,
    household_id         INTEGER,
    is_public            BOOLEAN     NOT NULL,
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
    FOREIGN KEY (reviewed_by) REFERENCES users (id),
    CHECK (
        (is_global IS TRUE AND is_public IS TRUE AND household_id IS NULL)              -- Global recipes
            OR
        (is_global IS FALSE AND is_public IS FALSE AND household_recipe_id IS NULL)     -- Household recipes
            OR
        (is_global IS FALSE AND is_public IS TRUE AND household_recipe_id IS NOT NULL)  -- Public recipes
            OR
        (is_global IS FALSE AND is_public IS FALSE AND household_recipe_id IS NOT NULL) -- Draft recipes
        )
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

-- SAVED RECIPES

CREATE TABLE saved_recipes
(
    id          SERIAL PRIMARY KEY,
    user_id     INTEGER NOT NULL,
    household_id INTEGER NOT NULL,
    recipe_id   INTEGER NOT NULL,
    created_at  TIMESTAMP NOT NULL,
    FOREIGN KEY (user_id) REFERENCES users (id),
    FOREIGN KEY (household_id) REFERENCES households (id),
    FOREIGN KEY (recipe_id) REFERENCES recipes (id)
);
