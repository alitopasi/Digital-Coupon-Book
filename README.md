# Irbid Offers | أوفرات إربد

Digital coupon book and local offers marketplace for Irbid, Jordan.

Irbid Offers transforms the traditional printed coupon book into a digital platform where customers can purchase offer books, access digital vouchers, and redeem them at participating businesses.

The system also provides tools for participating businesses to validate vouchers and monitor their performance, while administrators manage the entire platform.

---

## 🚀 Tech Stack

### Frontend
- React
- JavaScript / TypeScript — to be finalized
- Responsive Web Application
- REST API integration

### Backend
- **TBD**

### Database
- PostgreSQL

### External Services
- WhatsApp API — OTP and gift notifications
- Payment Gateway — Card & Apple Pay
- Push Notifications
- Maps / Location Services

---

## 👥 User Roles

The platform supports three main roles:

### Customer

Customers can:

- Register and log in using their mobile number and WhatsApp OTP
- Browse available coupon books
- Browse vouchers and offers
- Search and filter offers
- Purchase digital coupon books
- Apply promo codes
- Buy a book as a gift
- View purchased books
- View available vouchers
- Redeem vouchers using QR codes
- View remaining voucher uses
- View redemption history
- Track total savings
- View purchase history and receipts
- Manage their profile

### Provider

Participating businesses can:

- Access their provider account
- Manage branches and branch users
- Scan customer voucher QR codes
- Validate vouchers
- Confirm voucher redemptions
- View redemption history
- Monitor voucher usage
- View provider statistics and dashboard
- Monitor provider quota usage

A provider can have one main account and multiple branch users.

### Admin

The administrator can:

- Manage cities
- Manage categories
- Manage providers
- Manage provider branches
- Manage provider users
- Manage customers
- Create and manage coupon books
- Create and manage vouchers
- Configure voucher usage limits
- Configure provider quotas
- Manage orders
- Monitor payments
- Manage gifts
- Manage promo codes
- Monitor voucher redemptions
- View reports
- View platform statistics
- Manage application content
- Monitor system activity

---

# 📚 Core Business Concepts

## Book

A Book is a collection of vouchers sold together for a specific price.

Each book:

- Belongs to a city
- Contains multiple vouchers
- Has a selling price
- Has a validity period
- Has sale start and end dates
- Has a calculated total face value
- Can have a limited number of available copies

Possible statuses:

```text
Draft
Published
Sold Out
Unpublished
```

---

## Voucher

A Voucher represents one offer inside a book.

Each voucher:

- Belongs to one book
- Belongs to one provider
- Has an offer description
- Has a face value
- Has a usage limit
- Has a provider quota
- Has terms and conditions

Vouchers may be:

- Single-use
- Multi-use

Example:

```text
20% Discount
Provider: Restaurant X
Usage Limit: 3
Face Value: 5 JOD
```

---

## Purchased Book

After a successful payment, a customer receives their own copy of the purchased book.

A customer can purchase the same book more than once.

Each purchase creates a separate Purchased Book.

---

## Customer Voucher

When a book is purchased, the system creates customer-owned copies of the vouchers contained in that book.

Each Customer Voucher contains:

- Unique QR identification
- Usage limit
- Remaining uses
- Expiry date
- Current status

Possible statuses:

```text
Available
Partly Used
Used
Expired
```

---

## Redemption

A Redemption represents one confirmed use of a Customer Voucher.

Each redemption records:

- Customer voucher
- Provider
- Branch
- Provider staff user
- Date
- Time

Each confirmed redemption decreases the remaining uses by one.

---

# 🎁 Gifting

Customers can purchase books for another mobile number.

Gift flow:

```text
Customer
   │
   ▼
Select Book
   │
   ▼
Buy as Gift
   │
   ▼
Enter Recipient Mobile Number
   │
   ▼
Payment
   │
   ▼
Gift Created
   │
   ▼
WhatsApp Notification
   │
   ▼
Recipient Login / Registration
   │
   ▼
Book Added to Recipient Account
```

The validity period starts from the **purchase date**, not the date the gift is claimed.

---

# 🎟️ Promo Codes

Admin users can create promotional codes.

A promo code can contain:

- Code
- Discount type
- Discount value
- Validity start date
- Validity end date
- Total usage limit
- Per-customer usage limit
- Applicable books

Discount types:

```text
Percentage
Fixed Amount
```

Only one promo code can be applied to an order.

A promo code cannot reduce the final price to zero.

---

# 🏢 Provider Structure

```text
Provider
│
├── Main Account
│
├── Branch 1
│   └── Branch Users
│
├── Branch 2
│   └── Branch Users
│
└── Branch N
    └── Branch Users
```

The provider main account can view information across all branches.

A branch user can only access information related to their assigned branch.

---

# 💳 Payments

Supported payment methods:

- Card
- Apple Pay

Payment flow:

```text
Customer
   │
   ▼
Checkout
   │
   ▼
Payment Gateway
   │
   ▼
Payment Confirmation
   │
   ▼
Backend Verification
   │
   ▼
Order Paid
   │
   ▼
Purchased Book Created
   │
   ▼
Customer Vouchers Created
```

A book must only be activated after the backend receives successful payment confirmation.

Failed or cancelled payments must not create a Purchased Book.

---

# 🔐 Authentication

Authentication is based on:

```text
Mobile Number
      +
WhatsApp OTP
```

Login codes are sent through WhatsApp.

Authorization must be role-based.

Main roles:

```text
CUSTOMER
PROVIDER
ADMIN
```

A user may have more than one role where required.

---

# 🗄️ Database

Database:

```text
PostgreSQL
```

Main database entities currently identified from the requirements:

```text
users
user_roles

cities
categories

providers
branches
provider_users

books
vouchers

orders
payments

purchased_books
customer_vouchers
redemptions

gifts

promo_codes
promo_code_books
promo_code_usages

notifications
whatsapp_messages

audit_logs
```

The detailed PostgreSQL schema and migrations will be maintained separately from the README.

---

# 🏗️ High-Level Architecture

```text
┌─────────────────────────────┐
│                             │
│       React Frontend        │
│                             │
└──────────────┬──────────────┘
               │
               │ HTTPS / REST API
               │
               ▼
┌─────────────────────────────┐
│                             │
│          Backend            │
│            TBD              │
│                             │
└──────────────┬──────────────┘
               │
               ▼
┌─────────────────────────────┐
│                             │
│        PostgreSQL           │
│                             │
└─────────────────────────────┘

        External Services
               │
       ┌───────┼────────┐
       │       │        │
       ▼       ▼        ▼
   WhatsApp  Payment   Push
     API     Gateway   Notifications
```

---

# 📁 Repository Structure

The final structure will depend on the selected backend technology.

Initial proposed structure:

```text
irbid-offers/
│
├── frontend/
│
├── backend/
│
├── docs/
│   ├── requirements/
│   ├── database/
│   └── api/
│
├── .gitignore
├── README.md
└── LICENSE
```

---

# 🌿 Git Workflow

The project uses a feature-branch workflow.

Main branches:

```text
main
│
└── develop
     │
     ├── feature/...
     ├── fix/...
     └── refactor/...
```

## `main`

Contains stable, production-ready code.

Direct development on `main` should be avoided.

## `develop`

Contains the latest integrated development version.

Feature branches should normally be created from `develop`.

## Feature Branches

Naming convention:

```text
feature/<feature-name>
```

Examples:

```text
feature/authentication
feature/customer-home
feature/book-details
feature/checkout
feature/provider-dashboard
feature/voucher-redemption
feature/admin-providers
```

## Bug Fixes

```text
fix/<issue-name>
```

Examples:

```text
fix/login-validation
fix/payment-callback
fix/voucher-expiration
```

## Refactoring

```text
refactor/<area>
```

Example:

```text
refactor/auth-service
```

---

# 🔄 Development Workflow

Before starting a new feature:

```bash
git checkout develop
git pull origin develop
```

Create a feature branch:

```bash
git checkout -b feature/customer-authentication
```

After completing the work:

```bash
git add .
git commit -m "feat: implement customer authentication"
git push origin feature/customer-authentication
```

Then create a Pull Request:

```text
feature/customer-authentication
            ↓
         develop
```

After review and approval, the feature can be merged into `develop`.

---

# 📝 Commit Convention

Use clear commit messages.

### New Feature

```text
feat: add customer authentication
```

### Bug Fix

```text
fix: prevent expired voucher redemption
```

### Database

```text
db: add voucher redemption tables
```

### Documentation

```text
docs: update database schema
```

### Refactoring

```text
refactor: reorganize authentication service
```

### Styling

```text
style: improve customer dashboard layout
```

---

# 🔀 Pull Requests

Each major feature should be developed in its own branch and submitted through a Pull Request.

A Pull Request should include:

- What was implemented
- Why the change was needed
- Related issue
- Database changes, if any
- API changes, if any
- Screenshots for UI changes
- Testing notes

Code should be reviewed before merging into `develop`.

---

# 📋 GitHub Issues

Development tasks should be tracked using GitHub Issues.

Example issues:

```text
[Frontend] Create login page

[Frontend] Create customer home page

[Frontend] Create book details page

[Backend] Implement authentication API

[Backend] Implement books API

[Database] Create initial PostgreSQL schema

[Provider] Implement QR redemption flow

[Admin] Implement provider management

[Payment] Integrate payment gateway
```

---

# 🧩 Development Phases

## Phase 1 — Project Setup

- Repository setup
- Git workflow
- React project setup
- Backend project setup
- PostgreSQL setup
- Environment configuration
- Development standards

## Phase 2 — Core Database

- Users
- Roles
- Cities
- Categories
- Providers
- Branches
- Books
- Vouchers

## Phase 3 — Authentication

- Customer authentication
- Provider authentication
- Role-based authorization
- WhatsApp OTP

## Phase 4 — Customer Features

- Home
- Books
- Vouchers
- Search
- Customer profile
- Purchased books

## Phase 5 — Orders & Payments

- Checkout
- Promo codes
- Orders
- Payment gateway
- Purchased book creation

## Phase 6 — Voucher Redemption

- QR generation
- QR scanning
- Voucher validation
- Redemption
- Multi-use vouchers
- Redemption history

## Phase 7 — Provider Features

- Provider dashboard
- Branch management
- Branch users
- Redemption statistics
- Provider quota monitoring

## Phase 8 — Gifts

- Buy as gift
- WhatsApp gift notification
- Gift claiming
- Gift history

## Phase 9 — Admin

- Provider management
- Customer management
- Book management
- Voucher management
- Promo codes
- Orders
- Payments
- Reports

## Phase 10 — Testing & Release

- Unit testing
- Integration testing
- Security testing
- Performance testing
- User acceptance testing
- Production deployment

---

# 📖 Documentation

Project documentation should be stored under:

```text
/docs
```

Recommended structure:

```text
docs/
│
├── requirements/
│   └── business-requirements.pdf
│
├── database/
│   ├── erd.pdf
│   └── database-schema.pdf
│
└── api/
    └── api-documentation.md
```

---

# ⚙️ Environment Variables

Secrets must never be committed to GitHub.

Use:

```text
.env
```

for local configuration.

Provide:

```text
.env.example
```

for required environment variables without real credentials.

Examples:

```text
DATABASE_URL=

WHATSAPP_API_KEY=

PAYMENT_GATEWAY_KEY=

JWT_SECRET=

PUSH_NOTIFICATION_KEY=
```

The exact variables will be finalized after the backend and external services are selected.

---

# 🔒 Security

The project should follow these principles:

- HTTPS only
- Server-side authorization
- Role-based access control
- Secure OTP verification
- No card details stored in the application database
- Rate limiting for authentication and voucher redemption
- Secure payment webhook verification
- Atomic voucher redemption
- Environment secrets must never be committed
- Provider users must only access authorized provider/branch data

---

# 🌍 Localization

The platform supports:

- Arabic
- English

Arabic interfaces must support RTL layouts.

Currency:

```text
JOD
```

Application timezone:

```text
Asia/Amman
```

---

# 📌 Project Status

🚧 **Initial Development**

Current focus:

```text
Requirements
     ↓
Database Design
     ↓
Repository Setup
     ↓
Project Architecture
     ↓
Development
```

---

# 👨‍💻 Team

Developed by the **Irbid Offers Development Team**.

All team members should follow the agreed Git workflow, coding standards, database schema, and business requirements before merging changes.
