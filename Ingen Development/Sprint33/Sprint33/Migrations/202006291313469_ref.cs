namespace Sprint33.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _ref : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Referrals",
                c => new
                    {
                        refferal_ID = c.Int(nullable: false, identity: true),
                        referral_Doctors_Name = c.String(),
                        referral_doctor_Add = c.String(),
                        referral_doctor_num = c.String(),
                        referral_doctor_Email = c.String(),
                        referral_patient_Name = c.String(nullable: false),
                        referral_patient_Surname = c.String(nullable: false),
                        referral_patient_DOB = c.DateTime(nullable: false),
                        referral_patient_Gender = c.String(),
                        referral_ValidDate = c.DateTime(nullable: false),
                        refferal_Location = c.String(),
                        referral_Reasoning = c.String(),
                    })
                .PrimaryKey(t => t.refferal_ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Referrals");
        }
    }
}
