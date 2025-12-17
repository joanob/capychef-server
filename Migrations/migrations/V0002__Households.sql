-- HOUSEHOLDS

CREATE TABLE households
(
    id             SERIAL PRIMARY KEY,
    created_at     TIMESTAMP NOT NULL,
    row_version    INTEGER   NOT NULL,
    is_deleted     BOOLEAN   NOT NULL,
    deleted_at     TIMESTAMP,
    owner_id INTEGER NOT NULL,
    name       TEXT      NOT NULL,
    public_id       TEXT UNIQUE   NOT NULL,
    FOREIGN KEY (owner_id) REFERENCES users (id)
);

CREATE RULE "households_soft_deletion" AS ON DELETE TO "households" DO INSTEAD (
    UPDATE households
    SET is_deleted = true
    WHERE id = old.id
      AND NOT is_deleted
    );
