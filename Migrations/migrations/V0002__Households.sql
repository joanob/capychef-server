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

CREATE VIEW active_households AS
SELECT *
FROM households
WHERE is_deleted = FALSE;
                                                    
CREATE TABLE household_members (
  id SERIAL PRIMARY KEY,
  created_at     TIMESTAMP NOT NULL,
  row_version    INTEGER   NOT NULL,
  is_deleted     BOOLEAN   NOT NULL,
  deleted_at     TIMESTAMP,
  household_id INTEGER NOT NULL,
  user_id INTEGER NOT NULL,
  FOREIGN KEY (household_id) REFERENCES households (id),
  FOREIGN KEY (user_id) REFERENCES users (id)
);

CREATE INDEX idx_household_members_household_id ON household_members(household_id);
CREATE INDEX idx_household_members_user_id ON household_members(user_id);

CREATE RULE "household_members_soft_deletion" AS ON DELETE TO "household_members" DO INSTEAD (
    UPDATE household_members
    SET is_deleted = true
    WHERE id = old.id
      AND NOT is_deleted
    );
                
                                                         
CREATE TABLE household_invitations (
     id SERIAL PRIMARY KEY,
     created_at     TIMESTAMP NOT NULL,
     row_version    INTEGER   NOT NULL,
     is_deleted     BOOLEAN   NOT NULL,
     deleted_at     TIMESTAMP,
     household_id INTEGER NOT NULL,
     user_id INTEGER NOT NULL,
     is_answered BOOLEAN NOT NULL,
     answered_at     TIMESTAMP,
     is_accepted BOOLEAN NOT NULL,
     FOREIGN KEY (household_id) REFERENCES households (id),
     FOREIGN KEY (user_id) REFERENCES users (id)
);

CREATE RULE "household_invitations_soft_delete" AS ON DELETE TO "household_invitations" DO INSTEAD (
    UPDATE household_invitations
    SET is_deleted = true
    WHERE id = old.id
      AND NOT is_deleted
    );

CREATE TABLE household_join_requests (
   id SERIAL PRIMARY KEY,
   household_id INTEGER NOT NULL,
   user_id INTEGER NOT NULL,
   created_at     TIMESTAMP NOT NULL,
   row_version    INTEGER   NOT NULL,
   is_deleted     BOOLEAN   NOT NULL,
   deleted_at     TIMESTAMP,
   is_answered BOOLEAN NOT NULL,
   answered_at     TIMESTAMP,
   is_accepted BOOLEAN NOT NULL,
   is_hidden BOOLEAN NOT NULL DEFAULT FALSE,
   FOREIGN KEY (household_id) REFERENCES households (id),
   FOREIGN KEY (user_id) REFERENCES users (id)
);

CREATE RULE "household_join_requests_soft_delete" AS ON DELETE TO "household_join_requests" DO INSTEAD (
    UPDATE household_join_requests
    SET is_deleted = true
    WHERE id = old.id
      AND NOT is_deleted
    );

-- STORAGE SPACES

CREATE TABLE storage_spaces (
    id SERIAL PRIMARY KEY,
    name TEXT NOT NULL,
    storage_condition CHAR(1) NOT NULL,
    household_id INTEGER NOT NULL,
    created_at              TIMESTAMP NOT NULL,
    created_by              INTEGER,
    row_version             INTEGER   NOT NULL,
    is_deleted              BOOLEAN   NOT NULL,
    deleted_at              TIMESTAMP,
    FOREIGN KEY (household_id) REFERENCES households (id),
    FOREIGN KEY (created_by) REFERENCES users (id),
    CHECK (storage_condition IN ('A', 'R', 'F')) -- A = ambient, R = refrigerated, F = frozen
);

CREATE INDEX idx_storage_spaces_household_id ON storage_spaces(household_id);

CREATE RULE "storage_spaces_soft_deletion" AS ON DELETE TO "storage_spaces" DO INSTEAD (
    UPDATE storage_spaces
    SET is_deleted = true
    WHERE id = old.id
      AND NOT is_deleted
    );

CREATE VIEW active_storage_spaces AS
SELECT *
FROM storage_spaces
WHERE is_deleted = FALSE;

CREATE TABLE storage_spaces_modifications_history
(
    id      SERIAL PRIMARY KEY,
    storage_space_id INTEGER NOT NULL,
    column_name TEXT NOT NULL,
    previous_value TEXT,
    new_value TEXT,
    modified_at TIMESTAMP NOT NULL,
    modified_by INTEGER NOT NULL,
    FOREIGN KEY (storage_space_id) REFERENCES storage_spaces (id),
    FOREIGN KEY (modified_by) REFERENCES users (id)
);

CREATE INDEX idx_storage_spaces_modifications_history_food_id ON storage_spaces_modifications_history(storage_space_id);

CREATE RULE "storage_spaces_modifications_history_soft_deletion" AS ON DELETE TO "storage_spaces_modifications_history" DO INSTEAD NOTHING;