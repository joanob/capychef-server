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
   FOREIGN KEY (household_id) REFERENCES households (id),
   FOREIGN KEY (user_id) REFERENCES users (id)
);

CREATE RULE "household_join_requests_soft_delete" AS ON DELETE TO "household_join_requests" DO INSTEAD (
    UPDATE household_join_requests
    SET is_deleted = true
    WHERE id = old.id
      AND NOT is_deleted
    );