-- SUBSCRIPTION DISCOUNTS 

CREATE TABLE subscription_discounts
(
    id                   SERIAL PRIMARY KEY,
    discount_code        VARCHAR(50) NOT NULL UNIQUE,
    discount_type        VARCHAR(20) NOT NULL CHECK (discount_type IN ('PERCENTAGE', 'FREE_MONTH', 'FIXED_AMOUNT')),
    percentage_or_amount REAL        NOT NULL,
    valid_from           TIMESTAMP   NOT NULL,
    valid_until          TIMESTAMP   NOT NULL
);

CREATE INDEX idx_discounts_discount_code ON subscription_discounts(discount_code);

-- SUBSCRIPTIONS

CREATE TABLE subscriptions
(
    id                      SERIAL PRIMARY KEY,
    user_id                 INTEGER    NOT NULL,
    household_id            INTEGER,
    purchased_at            TIMESTAMP  NOT NULL,
    valid_from              TIMESTAMP  NOT NULL,
    expires_at              TIMESTAMP  NOT NULL,
    subscription_type       VARCHAR(1) NOT NULL CHECK (subscription_type IN ('M', 'A')),
    automatic_renewal       BOOLEAN    NOT NULL,
    is_primary_subscription BOOLEAN    NOT NULL,
    primary_subscription_id INTEGER,
    discount_id             INTEGER,
    amount_payed            REAL       NOT NULL,
    amount_saved            REAL       NOT NULL,
    FOREIGN KEY (user_id) REFERENCES users (id),
    FOREIGN KEY (primary_subscription_id) REFERENCES subscriptions (id),
    FOREIGN KEY (household_id) REFERENCES households (id),
    FOREIGN KEY (discount_id) REFERENCES subscription_discounts (id)
);

CREATE INDEX idx_subscriptions_user_id ON subscriptions(user_id);

CREATE INDEX idx_subscriptions_household_id ON subscriptions(household_id);

CREATE RULE "subscriptions_soft_deletion" AS ON DELETE TO "subscriptions" DO INSTEAD NOTHING;