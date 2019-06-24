DROP DATABASE yourshares
GO

CREATE DATABASE yourshares
GO

USE yourshares
GO

CREATE TABLE yourshares.dbo.user_profile (
  user_profile_id uniqueidentifier NOT NULL,
  first_name varchar(50) NULL,
  last_name varchar(50) NULL,
  email varchar(100) NULL,
  phone varchar(15) NULL,
  address varchar(200) NULL,
  CONSTRAINT PK_user_profile_user_profile_id PRIMARY KEY CLUSTERED (user_profile_id)
)
ON [PRIMARY]
GO

CREATE TABLE yourshares.dbo.user_account (
  user_profile_id uniqueidentifier NOT NULL,
  email varchar(200) NOT NULL,
  password_hash varchar(200) NOT NULL,
  password_hash_algorithm varchar(10) NOT NULL,
  password_reminder_token varchar(100) NULL,
  email_confirmation_token varchar(100) NULL,
  user_account_status_code varchar(10) NOT NULL,
  CONSTRAINT PK_user_account_user_profile_id PRIMARY KEY CLUSTERED (user_profile_id)
)
ON [PRIMARY]
GO

CREATE TABLE yourshares.dbo.google_account (
  user_profile_id uniqueidentifier NOT NULL,
  google_account_id varchar(30) NOT NULL,
  CONSTRAINT PK_google_account_user_profile_id PRIMARY KEY CLUSTERED (user_profile_id)
)
ON [PRIMARY]
GO

CREATE TABLE yourshares.dbo.company (
  company_id uniqueidentifier NOT NULL,
  admin_profile_id uniqueidentifier NOT NULL,
  company_name varchar(50) NOT NULL,
  address varchar(50) NULL,
  phone varchar(15) NULL,
  capital bigint NOT NULL,
  total_shares bigint NOT NULL,
  option_poll_amount bigint NULL,
  CONSTRAINT PK_Company_Company_ID PRIMARY KEY CLUSTERED (company_id)
)
ON [PRIMARY]
GO

CREATE TABLE yourshares.dbo.shareholder (
  shareholder_id uniqueidentifier NOT NULL,
  company_id uniqueidentifier NOT NULL,
  user_id uniqueidentifier NOT NULL,
  shareholder_type_code varchar(10) NOT NULL,
  CONSTRAINT PK_Shareholder_Shareholder_ID PRIMARY KEY CLUSTERED (shareholder_id)
)
ON [PRIMARY]
GO

CREATE TABLE yourshares.dbo.share_account (
  share_account_id uniqueidentifier NOT NULL,
  shareholder_id uniqueidentifier NOT NULL,
  share_amount bigint NOT NULL,
  share_type_code varchar(10) NULL,
  CONSTRAINT PK_Share_Account_Share_Account_ID PRIMARY KEY CLUSTERED (share_account_id)
)
ON [PRIMARY]
GO

CREATE TABLE yourshares.dbo.restricted_share (
  share_account_id uniqueidentifier NOT NULL,
  assign_date timestamp,
  convertible_time bigint NOT NULL,
  convertible_ratio float NOT NULL
)
ON [PRIMARY]
GO

CREATE TABLE yourshares.dbo.[transaction] (
  transaction_id uniqueidentifier NOT NULL,
  share_account_id uniqueidentifier NOT NULL,
  transaction_amount bigint NOT NULL,
  transaction_date timestamp,
  transaction_type_code varchar(10) NOT NULL,
  transaction_value bigint NOT NULL,
  transaction_status_code varchar(10) NOT NULL,
  CONSTRAINT PK_Transaction_Transaction_ID PRIMARY KEY CLUSTERED (transaction_id)
)
ON [PRIMARY]
GO

CREATE TABLE yourshares.dbo.ref_share_type_code (
  share_type_code varchar(10) NULL,
  name varchar(50) NULL
)
ON [PRIMARY]
GO

CREATE TABLE yourshares.dbo.ref_shareholder_type_code (
  shareholder_type_code varchar(10) NOT NULL,
  name varchar(50) NULL,
  CONSTRAINT PK_ref_shareholder_type_code_shareholder_type_code PRIMARY KEY CLUSTERED (shareholder_type_code)
)
ON [PRIMARY]
GO

CREATE TABLE yourshares.dbo.ref_transaction_status_code (
  transaction_status_code varchar(10) NOT NULL,
  name varchar(50) NULL,
  CONSTRAINT PK_ref_transaction_status_code_transaction_status_code PRIMARY KEY CLUSTERED (transaction_status_code)
)
ON [PRIMARY]
GO

CREATE TABLE yourshares.dbo.ref_transaction_type_code (
  transaction_type_code varchar(10) NULL,
  name varchar(50) NULL
)
ON [PRIMARY]
GO

CREATE TABLE yourshares.dbo.ref_user_account_status_code (
  user_account_status_code varchar(10) NOT NULL,
  name varchar(50) NULL,
  CONSTRAINT PK_ref_user_account_status_code_user_account_status_code PRIMARY KEY CLUSTERED (user_account_status_code)
)
ON [PRIMARY]
GO

INSERT INTO dbo.ref_share_type_code (share_type_code, name)
  VALUES ('PRF01', 'Preference'),
  ('STD02','Standard'),
  ('RST03','Restricted')

INSERT INTO dbo.ref_shareholder_type_code (shareholder_type_code, name)
  VALUES ('FD', 'Founders'),
  ('SH','Shareholders'),
  ('EMP','Employees')

INSERT INTO dbo.ref_transaction_status_code (transaction_status_code, name)
  VALUES ('RQ','Requested'),
  ('PD', 'Pending'),
  ('ACP', 'Accepted'),
  ('CMP', 'Completed')
  
INSERT INTO dbo.ref_transaction_type_code (transaction_type_code, name)
  VALUES ('DBT', 'Debit'),
  ('CRD', 'Credit')

INSERT INTO dbo.ref_user_account_status_code (user_account_status_code, name)
  VALUES ('GST', 'Guest'),
  ('USR', 'Verified user')

INSERT INTO dbo.user_profile (user_profile_id, first_name, last_name, email, phone, address)
  VALUES ('a9bdf9af-f6ff-471e-8c21-d618e2950f04', 'Tu', 'Nguyen', 'tunguyen@gmail.com', 0598643187, 'somewhere'),
  ('54eaf8e7-7566-4c8d-a467-ee94e158d975', 'Phu', 'Cao', 'phucao@gmail.com', 0985671493, 'myhouse'),
  ('867896ec-690c-4fff-8725-8abb3ccf59d0', 'Binh', 'Phan', 'binhphan@gmail.com', 0865974168, 'nohome')

INSERT INTO dbo.user_account (user_profile_id, email, password_hash, password_hash_algorithm, password_reminder_token, email_confirmation_token, user_account_status_code)
  VALUES ('a9bdf9af-f6ff-471e-8c21-d618e2950f04', 'tunguyen@gmail.com', 'nothashyet', 'no', DEFAULT, DEFAULT, 'USR'),
  ('54eaf8e7-7566-4c8d-a467-ee94e158d975', 'phucao@gmail.com', 'nothashyet', 'no', DEFAULT, DEFAULT, 'USR'),
  ('867896ec-690c-4fff-8725-8abb3ccf59d0', 'binhphan@gmail.com', 'nothashyet', 'no', DEFAULT, DEFAULT, 'USR')

INSERT INTO dbo.company (company_id, admin_profile_id, company_name, address, phone, capital, total_shares, option_poll_amount)
  VALUES ('80699ae2-fce7-4566-8732-803f7151e73c', '54eaf8e7-7566-4c8d-a467-ee94e158d975', 'NoNameComp', 'skyhigh', 0549751369, 777777777, 10000000, DEFAULT)