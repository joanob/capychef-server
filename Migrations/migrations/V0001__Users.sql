-- USERS

CREATE TABLE users
(
    id             SERIAL PRIMARY KEY,
    username       VARCHAR(50) NOT NULL,
    is_guest       BOOLEAN     NOT NULL,
    email          VARCHAR(255),
    is_email_valid BOOLEAN     NOT NULL,
    is_blocked     BOOLEAN     NOT NULL,
    block_reason   VARCHAR(5000),
    created_at     TIMESTAMP   NOT NULL,
    row_version    INTEGER     NOT NULL,
    is_deleted     BOOLEAN     NOT NULL,
    deleted_at     TIMESTAMP
);

CREATE UNIQUE INDEX idx_users_username ON users(username);
CREATE INDEX idx_users_email ON users(email);

-- USER PASSWORDS

CREATE TABLE users_passwords
(
    id         SERIAL PRIMARY KEY,
    user_id    INTEGER      NOT NULL,
    password   VARCHAR(255) NOT NULL,
    created_at TIMESTAMP    NOT NULL,
    is_active  BOOLEAN      NOT NULL,
    FOREIGN KEY (user_id) REFERENCES users (id)
);

CREATE INDEX idx_users_passwords_user_id ON users_passwords(user_id);

-- USERS SESSIONS

CREATE TABLE users_sessions
(
    id              SERIAL PRIMARY KEY,
    user_id         INTEGER   NOT NULL,
    created_at      TIMESTAMP NOT NULL,
    last_refresh_at TIMESTAMP NOT NULL,
    is_revoked      BOOLEAN   NOT NULL,
    FOREIGN KEY (user_id) REFERENCES users (id)
);

CREATE INDEX idx_users_sessions_user_id ON users_sessions(user_id);

-- USERS TOKENS

CREATE TABLE users_tokens
(
    id         SERIAL PRIMARY KEY,
    user_id    INTEGER     NOT NULL,
    token      VARCHAR(20) NOT NULL,
    token_type INTEGER     NOT NULL,
    created_at TIMESTAMP   NOT NULL,
    expires_at TIMESTAMP,
    is_active  BOOLEAN     NOT NULL,
    is_used    BOOLEAN     NOT NULL,
    used_at    TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users (id)
);

CREATE INDEX idx_users_tokens_token ON users_tokens(token);
