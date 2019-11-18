using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Examportal.Models
{
    public partial class ExamportalContext : DbContext
    {
        public ExamportalContext()
        {
        }

        public ExamportalContext(DbContextOptions<ExamportalContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CandidateAnswer> CandidateAnswer { get; set; }
        public virtual DbSet<CandidateResult> CandidateResult { get; set; }
        public virtual DbSet<ExamDetails> ExamDetails { get; set; }
        public virtual DbSet<Questions> Questions { get; set; }
        public virtual DbSet<Users> Users { get; set; }
      

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.

                optionsBuilder.UseSqlServer("Server=CYG356;Database=Examportal;Trusted_Connection=True;");

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CandidateAnswer>(entity =>
            {
                entity.HasKey(e => e.CandidateId);

                entity.ToTable("candidateAnswer");

                entity.Property(e => e.CandidateId).HasColumnName("candidateId");

                entity.Property(e => e.Answer)
                    .HasColumnName("answer")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CompletionTime)
                    .HasColumnName("completionTime")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CorrectStatus).HasColumnName("correctStatus");

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("createdDate")
                    .HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Id).HasColumnName("_id");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modifiedBy")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modifiedDate")
                    .HasColumnType("date");

                entity.Property(e => e.TestCode)
                    .HasColumnName("testCode")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.CandidateAnswer)
                    .HasForeignKey(d => d.Id)
                    .HasConstraintName("FK__candidateAn___id__06CD04F7");

                entity.HasOne(d => d.TestCodeNavigation)
                    .WithMany(p => p.CandidateAnswer)
                    .HasForeignKey(d => d.TestCode)
                    .HasConstraintName("FK__candidate__testC__07C12930");
            });

            modelBuilder.Entity<CandidateResult>(entity =>
            {
                entity.HasKey(e => e.TestCode);

                entity.ToTable("candidateResult");

                entity.Property(e => e.TestCode)
                    .HasColumnName("testCode")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SubmitExam).HasColumnName("submitExam");

                entity.Property(e => e.TotalScore).HasColumnName("totalScore");
            });

            modelBuilder.Entity<ExamDetails>(entity =>
            {
                entity.HasKey(e => e.ExamCode);

                entity.ToTable("examDetails");

                entity.Property(e => e.ExamCode)
                    .HasColumnName("examCode")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("createdDate")
                    .HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ExamDuration)
                    .HasColumnName("examDuration")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ExamName)
                    .HasColumnName("examName")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ExamStartTime)
                    .HasColumnName("examStartTime")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Id)
                    .HasColumnName("_id")
                    .ValueGeneratedOnAdd();

               // entity.Property(e => e.ModifiedBy)
                //    .HasColumnName("modifiedBy")
                  //  .HasMaxLength(20)
                   // .IsUnicode(false);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modifiedDate")
                    .HasColumnType("date");

                entity.HasOne(d => d.EmailNavigation)
                    .WithMany(p => p.ExamDetails)
                    .HasForeignKey(d => d.Email)
                    .HasConstraintName("FK__examDetai__email__66603565");
            });

            modelBuilder.Entity<Questions>(entity =>
            {
                entity.ToTable("questions");

                entity.Property(e => e.Id).HasColumnName("_id");

                entity.Property(e => e.Answer)
                    .HasColumnName("answer")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AnswerType)
                    .HasColumnName("answerType")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("createdDate")
                    .HasColumnType("date");

                entity.Property(e => e.ExamCode)
                    .HasColumnName("examCode")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modifiedBy")
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modifiedDate")
                    .HasColumnType("date");

                entity.Property(e => e.Option1)
                    .HasColumnName("option1")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Option2)
                    .HasColumnName("option2")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Option3)
                    .HasColumnName("option3")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Option4)
                    .HasColumnName("option4")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.QuestionImage)
                    .HasColumnName("questionImage")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.QuestionText)
                    .HasColumnName("questionText")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Weightage).HasColumnName("weightage");

                entity.HasOne(d => d.ExamCodeNavigation)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.ExamCode)
                    .HasConstraintName("FK__questions__examC__693CA210");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.Email);

                entity.ToTable("users");

                entity.HasIndex(e => e.PhoneNumber)
                    .HasName("UQ__users__4849DA013426D3A4")
                    .IsUnique();

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.AccountType)
                    .HasColumnName("accountType")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CollegeName)
                    .HasColumnName("collegeName")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasColumnName("createdBy")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("createdDate")
                    .HasColumnType("date");

                entity.Property(e => e.ModifiedBy)
                    .HasColumnName("modifiedBy")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modifiedDate")
                    .HasColumnType("date");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phoneNumber")
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });
        }
    }
}
