-- UNITS OF MEASURE
    
CREATE TABLE uom_dimensions
(
    code VARCHAR(4) PRIMARY KEY,
    name VARCHAR(50) NOT NULL
);

CREATE TABLE uom
(
    code        VARCHAR(4) PRIMARY KEY,
    name        VARCHAR(50) NOT NULL,
    dimension   VARCHAR(4)  NOT NULL,
    base_uom    VARCHAR(4),
    numerator   INTEGER,
    denominator INTEGER,
    FOREIGN KEY (dimension) REFERENCES uom_dimensions (code),
    FOREIGN KEY (base_uom) REFERENCES uom (code) DEFERRABLE INITIALLY DEFERRED, -- Avoid foreign key errors on mass load
    CHECK (
        (base_uom IS NULL AND numerator IS NULL AND denominator IS NULL) OR
        (base_uom IS NOT NULL AND numerator IS NOT NULL AND denominator IS NOT NULL)
        )
);

-- FOOD CATEGORIES

CREATE TABLE food_categories
(
    id                 INTEGER PRIMARY KEY,
    name               VARCHAR(50) NOT NULL,
    is_leaf            BOOLEAN     NOT NULL,
    parent_category_id INTEGER,
    FOREIGN KEY (parent_category_id) REFERENCES food_categories (id)
);

-- FOOD
                                                    
CREATE TABLE food
(
    id                      SERIAL PRIMARY KEY,
    name                    VARCHAR(50) NOT NULL,
    category_id             INTEGER     NOT NULL,
    is_global               BOOLEAN     NOT NULL,
    global_id               VARCHAR,
    household_id            INTEGER,
    modified_global_food_id INTEGER,
    days_until_expiration   INTEGER,
    days_until_best_before  INTEGER,
    created_at              TIMESTAMP   NOT NULL,
    created_by              INTEGER,
    row_version             INTEGER     NOT NULL,
    is_deleted              BOOLEAN     NOT NULL,
    deleted_at              TIMESTAMP,
    FOREIGN KEY (category_id) REFERENCES food_categories (id),
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

-- FOOD HOUSEHOLD DETAILS

CREATE TABLE household_food_details
(
    id                     SERIAL PRIMARY KEY,
    created_at             TIMESTAMP NOT NULL,
    row_version            INTEGER   NOT NULL,
    household_id           INT       NOT NULL,
    food_id                INT       NOT NULL,
    min_quantity           REAL,
    min_quantity_uom       VARCHAR(4),
    days_until_expiration  INTEGER,
    days_until_best_before INTEGER,
    FOREIGN KEY (household_id) REFERENCES households (id),
    FOREIGN KEY (food_id) REFERENCES food (id),
    FOREIGN KEY (min_quantity_uom) REFERENCES uom (code)
);    
            
-- FOOD MODIFICATIONS HISTORY
                                  
CREATE TABLE food_modifications_history
(
    id             SERIAL PRIMARY KEY,
    food_id        INTEGER   NOT NULL,
    household_id   INTEGER   NOT NULL,
    column_name    VARCHAR(50) NOT NULL,
    previous_value VARCHAR(100),
    new_value      VARCHAR(100),
    modified_at    TIMESTAMP NOT NULL,
    modified_by    INTEGER   NOT NULL,
    FOREIGN KEY (food_id) REFERENCES food (id),
    FOREIGN KEY (household_id) REFERENCES households (id),
    FOREIGN KEY (modified_by) REFERENCES users (id)
);

CREATE INDEX idx_food_modifications_history_food_id ON food_modifications_history(food_id);

-- FOOD UOM

CREATE TABLE food_uom
(
    id                   SERIAL PRIMARY KEY,
    food_id              INTEGER    NOT NULL,
    uom                  varchar(4) NOT NULL,
    household_id         INTEGER,
    is_base              BOOLEAN    NOT NULL DEFAULT FALSE,
    base_uom             VARCHAR(4),
    numerator            INTEGER,
    denominator          INTEGER,
    is_approx_conversion BOOLEAN,
    created_at           TIMESTAMP  NOT NULL,
    created_by INTEGER,
    row_version          INTEGER    NOT NULL,
    is_deleted           BOOLEAN    NOT NULL,
    deleted_at           TIMESTAMP,
    FOREIGN KEY (food_id) REFERENCES food (id),
    FOREIGN KEY (base_uom) REFERENCES uom (code),
    FOREIGN KEY (created_at) REFERENCES users(id),
    CHECK (
        (
            base_uom IS NULL AND
            numerator IS NULL AND
            denominator IS NULL AND
            is_approx_conversion IS NULL
            ) OR (
            base_uom IS NOT NULL AND
            numerator IS NOT NULL AND
            denominator IS NOT NULL AND
            is_approx_conversion IS NOT NULL
            )
        )
);

-- FOOD UOM MODIFICATIONS HISTORY

CREATE TABLE food_uom_modifications_history
(
    id             SERIAL PRIMARY KEY,
    food_uom_id        INTEGER   NOT NULL,
    column_name    VARCHAR(50) NOT NULL,
    previous_value VARCHAR(100),
    new_value      VARCHAR(100),
    modified_at    TIMESTAMP NOT NULL,
    modified_by    INTEGER   NOT NULL,
    FOREIGN KEY (food_uom_id) REFERENCES food_uom (id),
    FOREIGN KEY (modified_by) REFERENCES users (id)
);

CREATE INDEX idx_food_uom_modifications_history_food_id ON food_uom_modifications_history(food_uom_id);
