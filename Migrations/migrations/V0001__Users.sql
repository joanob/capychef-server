-- USERS

CREATE TABLE users
(
    id             SERIAL PRIMARY KEY,
    created_at     TIMESTAMP NOT NULL,
    row_version    INTEGER   NOT NULL,
    is_deleted     BOOLEAN   NOT NULL,
    deleted_at     TIMESTAMP,
    username       TEXT      NOT NULL,
    is_guest       BOOLEAN   NOT NULL,
    email          TEXT,
    is_email_valid BOOLEAN   NOT NULL,
    is_blocked     BOOLEAN   NOT NULL,
    block_reason   TEXT
);

CREATE UNIQUE INDEX idx_users_username ON users(username);
CREATE INDEX idx_users_email ON users(email);

CREATE RULE "users_soft_deletion" AS ON DELETE TO "users" DO INSTEAD (
    UPDATE users
    SET is_deleted = true
    WHERE id = old.id
      AND NOT is_deleted
    );


CREATE TABLE users_passwords
(
    id             SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL,
    password TEXT NOT NULL,
    created_at     TIMESTAMP NOT NULL,
    is_active     BOOLEAN   NOT NULL,
    FOREIGN KEY(user_id) REFERENCES users(id)
);

CREATE INDEX idx_users_passwords_user_id ON users_passwords(user_id);
CREATE INDEX idx_users_passwords_user_id_active ON users_passwords(user_id, is_active);

CREATE RULE "users_passwords_delete" AS ON DELETE TO "users_passwords" DO INSTEAD NOTHING;


CREATE TABLE users_sessions
(
    id             SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL,
    created_at     TIMESTAMP NOT NULL,
    last_connection_at     TIMESTAMP,
    is_revoked     BOOLEAN   NOT NULL,
    FOREIGN KEY(user_id) REFERENCES users(id)
);

CREATE INDEX idx_users_sessions_user_id ON users_sessions(user_id);

CREATE RULE "users_sessions_delete" AS ON DELETE TO "users_sessions" DO INSTEAD NOTHING;