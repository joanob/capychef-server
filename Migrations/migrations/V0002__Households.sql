-- HOUSEHOLDS

CREATE TABLE households
(
    id          SERIAL PRIMARY KEY,
    owner_id    INTEGER            NOT NULL,
    name        VARCHAR(50)        NOT NULL,
    public_id   VARCHAR(20) UNIQUE NOT NULL,
    created_at  TIMESTAMP          NOT NULL,
    created_by  INTEGER            NOT NULL,
    row_version INTEGER            NOT NULL,
    is_deleted  BOOLEAN            NOT NULL,
    deleted_at  TIMESTAMP,
    FOREIGN KEY (owner_id) REFERENCES users (id),
    FOREIGN KEY (created_by) REFERENCES users (id),
);

-- HOUSEHOLD MEMBERS
                                                    
CREATE TABLE household_members
(
    id           SERIAL PRIMARY KEY,
    household_id INTEGER   NOT NULL,
    user_id      INTEGER   NOT NULL,
    did_leave    BOOLEAN   NOT NULL,
    created_at   TIMESTAMP NOT NULL,
    row_version  INTEGER   NOT NULL,
    is_deleted   BOOLEAN   NOT NULL,
    deleted_at   TIMESTAMP,
    FOREIGN KEY (household_id) REFERENCES households (id),
    FOREIGN KEY (user_id) REFERENCES users (id)
);

CREATE INDEX idx_household_members_household_id ON household_members(household_id);
CREATE INDEX idx_household_members_user_id ON household_members(user_id);
     
-- HOUSEHOLD INVITATIONS
                                                
CREATE TABLE household_invitations
(
    id           SERIAL PRIMARY KEY,
    household_id INTEGER   NOT NULL,
    user_id      INTEGER   NOT NULL,
    is_answered  BOOLEAN   NOT NULL,
    answered_at  TIMESTAMP,
    is_accepted  BOOLEAN   NOT NULL,
    created_at   TIMESTAMP NOT NULL,
    row_version  INTEGER   NOT NULL,
    is_deleted   BOOLEAN   NOT NULL,
    deleted_at   TIMESTAMP,
    FOREIGN KEY (household_id) REFERENCES households (id),
    FOREIGN KEY (user_id) REFERENCES users (id)
);

-- HOUSEHOLD JOIN REQUESTS

CREATE TABLE household_join_requests
(
    id           SERIAL PRIMARY KEY,
    household_id INTEGER   NOT NULL,
    user_id      INTEGER   NOT NULL,
    created_at   TIMESTAMP NOT NULL,
    row_version  INTEGER   NOT NULL,
    is_deleted   BOOLEAN   NOT NULL,
    deleted_at   TIMESTAMP,
    is_answered  BOOLEAN   NOT NULL,
    answered_at  TIMESTAMP,
    is_accepted  BOOLEAN   NOT NULL,
    FOREIGN KEY (household_id) REFERENCES households (id),
    FOREIGN KEY (user_id) REFERENCES users (id)
);

-- STORAGE SPACES

CREATE TABLE storage_spaces
(
    id                SERIAL PRIMARY KEY,
    household_id      INTEGER     NOT NULL,
    name              VARCHAR(50) NOT NULL,
    storage_condition CHAR(1)     NOT NULL,
    created_at        TIMESTAMP   NOT NULL,
    created_by        INTEGER,
    row_version       INTEGER     NOT NULL,
    is_deleted        BOOLEAN     NOT NULL,
    deleted_at        TIMESTAMP,
    FOREIGN KEY (household_id) REFERENCES households (id),
    FOREIGN KEY (created_by) REFERENCES users (id),
    CHECK (storage_condition IN ('A', 'R', 'F')) -- A = ambient, R = refrigerated, F = frozen
);

CREATE INDEX idx_storage_spaces_household_id ON storage_spaces(household_id);

-- INITIAL STORAGE SPACES

CREATE TABLE initial_storage_spaces
(
    id                SERIAL PRIMARY KEY,
    name              VARCHAR(50)    NOT NULL,
    storage_condition CHAR(1) NOT NULL
);

CREATE INDEX idx_initial_storage_spaces_name ON initial_storage_spaces(name);

-- STORAGE SPACES MODIFICATIONS HISTORY

CREATE TABLE storage_spaces_modifications_history
(
    id               SERIAL PRIMARY KEY,
    storage_space_id INTEGER     NOT NULL,
    column_name      VARCHAR(50) NOT NULL,
    previous_value   VARCHAR(50),
    new_value        VARCHAR(50),
    modified_at      TIMESTAMP   NOT NULL,
    modified_by      INTEGER     NOT NULL,
    FOREIGN KEY (storage_space_id) REFERENCES storage_spaces (id),
    FOREIGN KEY (modified_by) REFERENCES users (id)
);

CREATE INDEX idx_storage_spaces_modifications_history_food_id ON storage_spaces_modifications_history(storage_space_id);
