-- CAPYCHEF GOURMET SUBSCRIPTION

-- Discounts 

CREATE TABLE discounts
(
    id SERIAL PRIMARY KEY,
    discount_code VARCHAR NOT NULL UNIQUE,
    discount_type VARCHAR NOT NULL CHECK (discount_type IN ('PERCENTAGE', 'FREE_MONTH' 'FIXED_AMOUNT')),
    percentage_or_amount REAL NOT NULL,
    valid_from TIMESTAMP NOT NULL,
    valid_until TIMESTAMP NOT NULL
);

CREATE INDEX idx_discounts_discount_code ON discounts(discount_code);

CREATE RULE "discounts_soft_deletion" AS ON DELETE TO "discounts" DO INSTEAD NOTHING;

-- Subscriptions

CREATE TABLE subscriptions
(
    id SERIAL PRIMARY KEY,
    user_id INTEGER NOT NULL,
    purchased_at TIMESTAMP NOT NULL,
    valid_from TIMESTAMP NOT NULL,
    expires_at TIMESTAMP NOT NULL,
    subscription_type VARCHAR NOT NULL CHECK (subscription_type IN ('M', 'A')),
    automatic_renewal BOOLEAN NOT NULL,
    is_primary_subscription BOOLEAN NOT NULL,
    primary_subscription_id INTEGER,
    household_id INTEGER,
    discount_id INTEGER,
    amount_payed REAL NOT NULL,
    amount_saved REAL NOT NULL,
    FOREIGN KEY (user_id) REFERENCES users (id),
    FOREIGN KEY (primary_subscription_id) REFERENCES subscriptions (id),
    FOREIGN KEY (household_id) REFERENCES households (id),
    FOREIGN KEY (discount_id) REFERENCES discounts (id)
);

CREATE INDEX idx_subscriptions_user_id ON subscriptions(user_id);

CREATE INDEX idx_subscriptions_household_id ON subscriptions(household_id);

CREATE RULE "subscriptions_soft_deletion" AS ON DELETE TO "subscriptions" DO INSTEAD NOTHING;