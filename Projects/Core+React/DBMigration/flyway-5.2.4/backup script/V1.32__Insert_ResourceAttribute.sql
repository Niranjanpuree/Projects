Insert into ResourceAttribute(Name,Title,ResourceType,AttributeType,VisibleToGrid,GridFieldOrder,ColumnWidth,ColumnMinimumWidth)
values('Code','Code','FarContractType','String',1,10,55,55),
('Title','Title','FarContractType','String',1,10,55,55),
('UpdatedByName','Updated By','FarContractType','String',1,40,55,55)


Insert into ResourceAttribute(ResourceAttributeGuid,Name,Title,ResourceType,AttributeType,VisibleToGrid)
values
('44D9B9AE-6E1B-4EEC-BF2E-90E13BC87F34','ModificationType','Modification Type','ContractModification','Combo',1)

Insert into ResourceAttributeValue (ResourceAttributeGuid,Name,Value)
values('44D9B9AE-6E1B-4EEC-BF2E-90E13BC87F34',
'Client Sponsored (All modifications initiated by the client- i.e. admin & unilateral mods, options, new work unsolicited)',
'ClientSponsored'),

('44D9B9AE-6E1B-4EEC-BF2E-90E13BC87F34',
'Client Requested (all bilateral modifications that are the result of a charge notice or other request that NW entity made & client agreed and issued a RFP)',
'ClientRequested'),

('44D9B9AE-6E1B-4EEC-BF2E-90E13BC87F34',
'Client Requested- REA (REA (all bilateral modifications resulting form a submitted REA or Contract Claim)',
'ClientRequested-REA'),

('44D9B9AE-6E1B-4EEC-BF2E-90E13BC87F34',
'NW Admin Mod Type I (funding related or otherwise change definitized but not formally processed)',
'NWAdminModTypeI'),

('44D9B9AE-6E1B-4EEC-BF2E-90E13BC87F34',
'NW Admin Mod Type II (basis of change agreed with client but value not definitized, e.g. could include notice to proceed or other form of letter agreement or direction)',
'NWAdminModTypeII'),

('44D9B9AE-6E1B-4EEC-BF2E-90E13BC87F34',
'NW Admin Mod Type III (RESERVED)',
'NWAdminModTypeIII')