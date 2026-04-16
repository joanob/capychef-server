-- SUPERMARKETS
                                                    
CREATE TABLE supermarkets
(
    id                             SERIAL PRIMARY KEY,
    name                           VARCHAR(50) NOT NULL,
    is_global                      BOOLEAN     NOT NULL,
    global_id                      VARCHAR,
    household_id                   INTEGER,
    modified_global_supermarket_id INTEGER,
    created_at                     TIMESTAMP   NOT NULL,
    created_by                     INTEGER,
    row_version                    INTEGER     NOT NULL,
    is_deleted                     BOOLEAN     NOT NULL,
    deleted_at                     TIMESTAMP,
    FOREIGN KEY (household_id) REFERENCES households (id),
    FOREIGN KEY (modified_global_supermarket_id) REFERENCES supermarkets (id),
    FOREIGN KEY (created_by) REFERENCES users (id),
    UNIQUE (household_id, modified_global_supermarket_id),
    CHECK (
        (is_global IS TRUE AND global_id IS NOT NULL AND household_id IS NULL AND modified_global_food_id IS NULL AND
         created_by IS NULL)
            OR
        (is_global IS FALSE AND global_id IS NULL AND household_id IS NOT NULL AND created_by IS NOT NULL)
        )
);

-- SUPERMARKET FOOD DETAILS

CREATE TABLE supermarket_food_details
(
    id                       SERIAL PRIMARY KEY,
    household_id             INT       NOT NULL,
    food_id                  INT       NOT NULL,
    supermarket_id           INT       NOT NULL,
    price                    REAL,
    quantity                 REAL,
    food_uom_id              INTEGER,
    is_preffered_supermarket BOOLEAN   NOT NULL,
    created_at               TIMESTAMP NOT NULL,
    created_by               INTEGER   NOT NULL,
    row_version              INTEGER   NOT NULL,
    is_deleted                     BOOLEAN     NOT NULL,
    deleted_at                     TIMESTAMP,
    FOREIGN KEY (household_id) REFERENCES households (id),
    FOREIGN KEY (food_id) REFERENCES food (id),
    FOREIGN KEY (supermarket_id) REFERENCES supermarkets (id),
    FOREIGN KEY (food_uom_id) REFERENCES food_uom (id)
);

CREATE INDEX idx_supermarket_food_details_household_id ON supermarket_food_details (household_id);

CREATE INDEX idx_supermarket_food_details_household_food ON supermarket_food_details (household_id, food_id);

CREATE INDEX idx_supermarket_food_details_household_supermarket ON supermarket_food_details (household_id, supermarket_id);

-- SUPERMARKET FOOD DETAILS MODIFICATIONS HISTORY

CREATE TABLE supermarket_food_details_modifications_history
(
    id                          SERIAL PRIMARY KEY,
    supermarket_food_details_id INTEGER     NOT NULL,
    price                       REAL,
    quantity                    REAL,
    food_uom_id                 INTEGER,
    is_preferred_supermarket    BOOLEAN     NOT NULL,
    created_at                  TIMESTAMP   NOT NULL,
    created_by                  INTEGER     NOT NULL,
    FOREIGN KEY (supermarket_food_details_id) REFERENCES supermarket_food_details (id),
    FOREIGN KEY (food_uom_id) REFERENCES food_uom (id),
    FOREIGN KEY (created_by) REFERENCES users (id)
);

CREATE INDEX idx_supermarket_food_details_modifications_history_details_id ON supermarket_food_details_modifications_history (supermarket_food_details_id);

-- SHOPPING LIST

CREATE TABLE shopping_list_items
(
    id                       SERIAL PRIMARY KEY,
    household_id             INT       NOT NULL,
    food_id                  INT,
    name                     VARCHAR(50),
    quantity                 REAL,
    food_uom_id              INTEGER,
    preferred_supermarket_id INTEGER,
    is_purchased             BOOLEAN   NOT NULL,
    purchased_at             TIMESTAMP,
    purchased_by             INTEGER,
    is_stored                BOOLEAN   NOT NULL,
    stored_at                TIMESTAMP,
    stored_by                INTEGER,
    created_at               TIMESTAMP NOT NULL,
    created_by               INTEGER   NOT NULL,
    row_version              INTEGER   NOT NULL,
    is_deleted                     BOOLEAN     NOT NULL,
    deleted_at                     TIMESTAMP,
    FOREIGN KEY (household_id) REFERENCES households (id),
    FOREIGN KEY (food_id) REFERENCES food (id),
    FOREIGN KEY (food_uom_id) REFERENCES food_uom (id),
    FOREIGN KEY (preferred_supermarket_id) REFERENCES supermarkets (id),
    FOREIGN KEY (purchased_by) REFERENCES users (id),
    FOREIGN KEY (stored_by) REFERENCES users (id),
    FOREIGN KEY (created_by) REFERENCES users (id),
    CHECK (
        -- If food_id is null, name must be provided, and vice versa
        ((food_id IS NOT NULL AND name IS NULL) OR
         (food_id IS NULL AND name IS NOT NULL))
            AND
            -- If quantity is provided, food_uom_id must also be provided, and vice versa
        ((quantity IS NOT NULL AND food_uom_id IS NOT NULL) OR
         (quantity IS NULL AND food_uom_id IS NULL))
            AND
            -- If item is marked as purchased, purchased_at and purchased_by must be provided, and vice versa
        ((is_purchased AND purchased_at IS NOT NULL AND purchased_by IS NOT NULL) OR
         (NOT is_purchased AND purchased_at IS NULL AND purchased_by IS NULL))
            AND
            -- If item is marked as stored, stored_at and stored_by must be provided, and vice versa
        ((is_stored AND stored_at IS NOT NULL AND stored_by IS NOT NULL) OR
         (NOT is_stored AND stored_at IS NULL AND stored_by IS NULL))
        )
);