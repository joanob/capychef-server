-- BATCHES

CREATE TABLE batches
(
    id                      SERIAL PRIMARY KEY,
    household_id            INTEGER NOT NULL,
    food_id INTEGER NOT NULL,
    storage_space_id INTEGER NOT NULL,
    quantity REAL NOT NULL,
    food_uom_id INTEGER NOT NULL,
    stored_at TIMESTAMP NOT NULL,
    original_batch_id INTEGER,
    is_consumed BOOLEAN NOT NULL,
    consumed_at TIMESTAMP,
    is_discarded BOOLEAN NOT NULL,
    discarded_at TIMESTAMP,
    created_at              TIMESTAMP NOT NULL,
    created_by              INTEGER,
    row_version             INTEGER   NOT NULL,
    is_deleted              BOOLEAN   NOT NULL,
    deleted_at              TIMESTAMP,
    FOREIGN KEY (household_id) REFERENCES households (id),
    FOREIGN KEY (food_id) REFERENCES food (id),
    FOREIGN KEY (storage_space_id) REFERENCES storage_spaces(id),
    FOREIGN KEY (food_uom_id) REFERENCES food_uom(id),
    FOREIGN KEY (original_batch_id) REFERENCES batches(id),
    FOREIGN KEY (created_by) REFERENCES users (id),
    CHECK (
        -- Batch cannot be consumed and discarded
        NOT (is_consumed AND is_discarded)
        -- Batch consumed <-> batch consumed date
        AND (
            (is_consumed AND consumed_at IS NOT NULL) OR
            (NOT is_consumed AND consumed_at IS NULL)
            )
            -- Batch discarded <-> batch discarded date
        AND (
            (is_discarded AND discarded_at IS NOT NULL) OR
            (NOT is_discarded AND discarded_at IS NULL)
            )
        )
);

CREATE INDEX idx_batches_household_id ON batches(household_id);

CREATE RULE "batches_soft_deletion" AS ON DELETE TO "batches" DO INSTEAD (
    UPDATE batches
    SET is_deleted = true
    WHERE id = old.id
      AND NOT is_deleted
    );

CREATE VIEW available_batches AS
SELECT *
FROM batches
WHERE is_consumed = FALSE
AND is_discarded = FALSE;