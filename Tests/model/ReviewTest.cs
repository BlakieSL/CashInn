using CashInn.Enum;
using CashInn.Model;

namespace Tests.model;

[TestFixture]
[TestOf(typeof(Review))]
public class ReviewTest
{
    private Review _review = null!;
    private const string TestFilePath = "Reviews.json";

    [SetUp]
    public void SetUp()
    {
        _review = new Review(1, Rating.Five, "Excellent service!");
        if (File.Exists(TestFilePath))
        {
            File.Delete(TestFilePath);
        }

        Review.ClearExtent();
        Review.LoadExtent();
    }

    [Test]
    public void Id_SetNegativeValue_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _review.Id = -1);
    }

    [Test]
    public void Id_SetPositiveValue_ShouldSet()
    {
        _review.Id = 2;
        Assert.That(_review.Id, Is.EqualTo(2));
    }

    [Test]
    public void Description_SetNull_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _review.Description = null);
    }

    [Test]
    public void Description_SetWhiteSpace_ShouldThrowException()
    {
        Assert.Throws<ArgumentException>(() => _review.Description = "  ");
    }

    [Test]
    public void Description_SetNonWhiteSpace_ShouldSet()
    {
        _review.Description = "Great food!";
        Assert.That(_review.Description, Is.EqualTo("Great food!"));
    }

    [Test]
    public void ReviewRating_SetValidValue_ShouldSet()
    {
        _review.ReviewRating = Rating.Four;
        Assert.That(_review.ReviewRating, Is.EqualTo(Rating.Four));
    }

    [Test]
    public void LoadExtent_ShouldRetrieveStoredReviewsCorrectly()
    {
        Review.ClearExtent();
        var review1 = new Review(1, Rating.Five, "Excellent service!");
        var review2 = new Review(2, Rating.Four, "Good food!");

        Review.SaveExtent();

        Assert.AreEqual(2, Review.GetAll().Count);

        review1 = null!;
        review2 = null!;
        Review.LoadExtent();

        Assert.AreEqual(2, Review.GetAll().Count);

        var loadedReviews = Review.GetAll();
        Assert.IsTrue(loadedReviews.Any(r => r.Id == 1));
        Assert.IsTrue(loadedReviews.Any(r => r.Id == 2));
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(TestFilePath))
        {
            File.Delete(TestFilePath);
        }
    }
}