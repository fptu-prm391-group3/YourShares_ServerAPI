DROP DATABASE yourshares
GO

CREATE DATABASE yourshares
GO

USE yourshares
GO

CREATE TABLE yourshares.dbo.user_profile (
  user_profile_id uniqueidentifier NOT NULL,
  first_name nvarchar(50) NOT NULL,
  last_name nvarchar(50) NOT NULL,
  email varchar(100) NULL,
  phone varchar(15) NULL,
  address nvarchar(200) NULL,
  CONSTRAINT PK_user_profile_user_profile_id PRIMARY KEY CLUSTERED (user_profile_id)
)
ON [PRIMARY]
GO

CREATE TABLE yourshares.dbo.user_account (
  user_profile_id uniqueidentifier NOT NULL,
  email varchar(200) NOT NULL,
  password_hash varchar(200) NOT NULL,
  password_hash_algorithm varchar(10) NOT NULL,
  password_salt uniqueidentifier NOT NULL,
  user_account_status_code varchar(10) NOT NULL,
  CONSTRAINT PK_user_account_user_profile_id PRIMARY KEY CLUSTERED (user_profile_id)
)
ON [PRIMARY]
GO

ALTER TABLE yourshares.dbo.user_account
  ADD CONSTRAINT FK_user_account_user_account_status_code FOREIGN KEY (user_account_status_code) REFERENCES dbo.ref_user_account_status_code (user_account_status_code)
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
  company_name nvarchar(50) NOT NULL,
  address nvarchar(50) NULL,
  phone varchar(15) NULL,
  capital bigint NOT NULL,
  total_shares bigint NOT NULL,
  option_poll_amount bigint NOT NULL,
  CONSTRAINT PK_Company_Company_ID PRIMARY KEY CLUSTERED (company_id)
)
ON [PRIMARY]
GO

CREATE TABLE yourshares.dbo.shareholder (
  shareholder_id uniqueidentifier NOT NULL,
  company_id uniqueidentifier NOT NULL,
  user_profile_id uniqueidentifier NOT NULL,
  shareholder_type_code varchar(10) NOT NULL,
  CONSTRAINT PK_Shareholder_Shareholder_ID PRIMARY KEY CLUSTERED (shareholder_id)
)
ON [PRIMARY]
GO

ALTER TABLE yourshares.dbo.shareholder
  ADD CONSTRAINT FK_shareholder_shareholder_type_code FOREIGN KEY (shareholder_type_code) REFERENCES dbo.ref_shareholder_type_code (shareholder_type_code)
GO

CREATE TABLE yourshares.dbo.share_account (
  share_account_id uniqueidentifier NOT NULL,
  shareholder_id uniqueidentifier NOT NULL,
  share_amount bigint NOT NULL,
  share_type_code varchar(10) NOT NULL,
  CONSTRAINT PK_Share_Account_Share_Account_ID PRIMARY KEY CLUSTERED (share_account_id)
)
ON [PRIMARY]
GO

ALTER TABLE yourshares.dbo.share_account
  ADD CONSTRAINT FK_share_account_share_type_code FOREIGN KEY (share_type_code) REFERENCES dbo.ref_share_type_code (share_type_code)
GO

CREATE TABLE yourshares.dbo.restricted_share (
  share_account_id uniqueidentifier NOT NULL,
  assign_date bigint NOT NULL,
  convertible_time bigint NOT NULL,
  convertible_ratio float NOT NULL,
  CONSTRAINT PK_restricted_share_share_account_id PRIMARY KEY CLUSTERED (share_account_id)
)
ON [PRIMARY]
GO

CREATE TABLE yourshares.dbo.[transaction] (
  transaction_id uniqueidentifier NOT NULL,
  share_account_id uniqueidentifier NOT NULL,
  transaction_amount bigint NOT NULL,
  transaction_date bigint NOT NULL,
  transaction_type_code varchar(10) NOT NULL,
  transaction_value bigint NOT NULL,
  transaction_status_code varchar(10) NOT NULL,
  CONSTRAINT PK_Transaction_Transaction_ID PRIMARY KEY CLUSTERED (transaction_id)
)
ON [PRIMARY]
GO

ALTER TABLE yourshares.dbo.[transaction]
  ADD CONSTRAINT FK_transaction_transaction_status_code FOREIGN KEY (transaction_status_code) REFERENCES dbo.ref_transaction_status_code (transaction_status_code)
GO

ALTER TABLE yourshares.dbo.[transaction]
  ADD CONSTRAINT FK_transaction_transaction_type_code FOREIGN KEY (transaction_type_code) REFERENCES dbo.ref_transaction_type_code (transaction_type_code)
GO

CREATE TABLE yourshares.dbo.transaction_request (
  transaction_request_id uniqueidentifier NOT NULL,
  transaction_in_id uniqueidentifier NOT NULL,
  transaction_out_id uniqueidentifier NOT NULL,
  approver_id uniqueidentifier NOT NULL,
  request_message nvarchar(max) NULL,
  CONSTRAINT PK_transaction_request_transaction_request_id PRIMARY KEY CLUSTERED (transaction_request_id)
)
ON [PRIMARY]
GO

CREATE TABLE yourshares.dbo.ref_share_type_code (
  share_type_code varchar(10) NOT NULL,
  name varchar(50) NOT NULL,
  CONSTRAINT PK_ref_share_type_code_share_type_code PRIMARY KEY CLUSTERED (share_type_code)
)
ON [PRIMARY]
GO

CREATE TABLE yourshares.dbo.ref_shareholder_type_code (
  shareholder_type_code varchar(10) NOT NULL,
  name varchar(50) NOT NULL,
  CONSTRAINT PK_ref_shareholder_type_code_shareholder_type_code PRIMARY KEY CLUSTERED (shareholder_type_code)
)
ON [PRIMARY]
GO

CREATE TABLE yourshares.dbo.ref_transaction_status_code (
  transaction_status_code varchar(10) NOT NULL,
  name varchar(50) NOT NULL,
  CONSTRAINT PK_ref_transaction_status_code_transaction_status_code PRIMARY KEY CLUSTERED (transaction_status_code)
)
ON [PRIMARY]
GO

CREATE TABLE yourshares.dbo.ref_transaction_type_code (
  transaction_type_code varchar(10) NOT NULL,
  name varchar(50) NOT NULL,
  CONSTRAINT PK_ref_transaction_type_code_transaction_type_code PRIMARY KEY CLUSTERED (transaction_type_code)
)
ON [PRIMARY]
GO

CREATE TABLE yourshares.dbo.ref_user_account_status_code (
  user_account_status_code varchar(10) NOT NULL,
  name varchar(50) NOT NULL,
  CONSTRAINT PK_ref_user_account_status_code_user_account_status_code PRIMARY KEY CLUSTERED (user_account_status_code)
)
ON [PRIMARY]
GO

CREATE TABLE yourshares.dbo.round (
  round_id uniqueidentifier NOT NULL,
  company_id uniqueidentifier NOT NULL,
  name nvarchar(50) NOT NULL,
  pre_round_shares bigint NOT NULL,
  post_round_shares bigint NOT NULL,
  CONSTRAINT PK_round_round_id PRIMARY KEY CLUSTERED (round_id)
)
ON [PRIMARY]
GO

CREATE TABLE yourshares.dbo.round_investor (
  round_investor_id uniqueidentifier NOT NULL,
  round_id uniqueidentifier NOT NULL,
  investor_name nvarchar(100) NOT NULL,
  share_amount bigint NOT NULL,
  invested_value bigint NOT NULL,
  shares_holding_percentage float NOT NULL,
  CONSTRAINT PK_round_investor_round_investor_id PRIMARY KEY CLUSTERED (round_investor_id)
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
  VALUES ('IN', 'Transaction In'),
  ('OUT', 'Transaction OUT')

INSERT INTO dbo.ref_user_account_status_code (user_account_status_code, name)
  VALUES ('GST', 'Guest'),
  ('USR', 'Verified user')

GO