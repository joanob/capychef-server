-- BATCHES

CREATE TABLE batches
(
    id                SERIAL PRIMARY KEY,
    household_id      INTEGER   NOT NULL,
    food_id           INTEGER   NOT NULL,
    storage_space_id  INTEGER   NOT NULL,
    quantity          REAL      NOT NULL,
    food_uom_id       INTEGER   NOT NULL,
    best_before_date  DATE,
    expiration_date   DATE,
    original_batch_id INTEGER,
    is_open           BOOLEAN   NOT NULL,
    opened_at         TIMESTAMP,
    is_consumed       BOOLEAN   NOT NULL,
    consumed_at       TIMESTAMP,
    is_discarded      BOOLEAN   NOT NULL,
    discarded_at      TIMESTAMP,
    created_at        TIMESTAMP NOT NULL,
    created_by        INTEGER   NOT NULL,
    row_version       INTEGER   NOT NULL,
    is_deleted        BOOLEAN   NOT NULL,
    deleted_at        TIMESTAMP,
    FOREIGN KEY (household_id) REFERENCES households (id),
    FOREIGN KEY (food_id) REFERENCES food (id),
    FOREIGN KEY (storage_space_id) REFERENCES storage_spaces (id),
    FOREIGN KEY (food_uom_id) REFERENCES food_uom (id),
    FOREIGN KEY (original_batch_id) REFERENCES batches (id),
    FOREIGN KEY (created_by) REFERENCES users (id),
    CHECK (
        NOT (is_consumed AND is_discarded)
            AND (
            (is_consumed AND consumed_at IS NOT NULL) OR
            (NOT is_consumed AND consumed_at IS NULL)
            )
            AND (
            (is_discarded AND discarded_at IS NOT NULL) OR
            (NOT is_discarded AND discarded_at IS NULL)
            )
        )
);

CREATE INDEX idx_batches_household_id ON batches(household_id);

-- BATCH MODIFICATIONS HISTORY 

CREATE TABLE batch_modifications_history
(
    id                   SERIAL PRIMARY KEY,
    batch_id             INTEGER    NOT NULL,
    modification_type    VARCHAR(1) NOT NULL,
    storage_space_id     INTEGER    NOT NULL,
    previous_quantity    REAL       NOT NULL,
    previous_food_uom_id INTEGER    NOT NULL,
    delta_quantity       REAL       NOT NULL,
    delta_food_uom_id    INTEGER    NOT NULL,
    new_quantity         REAL       NOT NULL,
    new_food_uom_id      INTEGER    NOT NULL,
    best_before_date     DATE,
    expiration_date      DATE,
    created_at           TIMESTAMP  NOT NULL,
    created_by           INTEGER,
    FOREIGN KEY (batch_id) REFERENCES batches (id),
    FOREIGN KEY (storage_space_id) REFERENCES storage_spaces (id),
    FOREIGN KEY (previous_food_uom_id) REFERENCES food_uom (id),
    FOREIGN KEY (delta_food_uom_id) REFERENCES food_uom (id),
    FOREIGN KEY (new_food_uom_id) REFERENCES food_uom (id),
    FOREIGN KEY (created_by) REFERENCES users (id)
);
