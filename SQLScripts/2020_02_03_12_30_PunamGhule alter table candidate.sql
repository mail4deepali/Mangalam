alter table candidate
drop column candidateid,
drop column userid,
drop column permanent_address,
drop column permanent_ad_countryid,
drop column permanent_ad_stateid,
drop column permanent_ad_districtid,
drop column permanent_ad_pincode,
drop column residential_address,
drop column residential_ad_countryid,
drop column residential_ad_stateid,
drop column residential_ad_districtid,
drop column residential_ad_pincode,
drop column mother_toungid;

ALTER TABLE candidate 
DROP CONSTRAINT candidate_casteid_fkey,
DROP CONSTRAINT candidate_subcasteid_fkey,
DROP CONSTRAINT candidate_education_id_fkey,
DROP CONSTRAINT candidate_color_complexionid_fkey,
DROP CONSTRAINT candidate_family_typeid_fkey,
DROP CONSTRAINT candidate_genderid_fkey,
DROP CONSTRAINT candidate_religionid_fkey;

ALTER TABLE candidate
  RENAME COLUMN genderid TO gender_id;

ALTER TABLE candidate
  RENAME COLUMN religionid TO religion_id;

ALTER TABLE candidate
  RENAME COLUMN casteid TO caste_id;
  
ALTER TABLE candidate
  RENAME COLUMN subcasteid TO sub_caste_id;
  
ALTER TABLE candidate
  RENAME COLUMN family_typeid TO family_type_id;
  
ALTER TABLE candidate
  RENAME COLUMN color_complexionid TO color_complexion_id;
  
ALTER TABLE candidate
  RENAME COLUMN contact_no TO phone_number;  
  
ALTER TABLE candidate
add id smallint primary key;

  