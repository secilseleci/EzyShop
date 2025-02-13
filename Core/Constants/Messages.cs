namespace Core.Constants
{
    public class Messages
    {
        #region Product Messages 
        public const string ProductNotFound = "Product not found";
        public const string EmptyProductList = "Product list is empty with the current filter";

        public const string CreateProductSuccess = "Product created successfully";
        public const string CreateProductError = "Error occurred while registering the Product to Database";

        public const string UpdateProductSuccess = "Product successfully updated";
        public const string UpdateProductError = "Error occurred while updating the Product in Database";

        public const string DeleteProductSuccess = "Product successfully deleted";
        public const string DeleteProductError = "Error occurred while deleting the Product from Database";

        public const string NoProductFilters = "No products found matching the filters.";
        #endregion

        #region Category Messages
        public const string CategoryNotFound = "Category not found";
        public const string EmptyCategoryList = "Category list is empty with the current filter";

        public const string CreateCategorySuccess = "Category created successfully";
        public const string CreateCategoryError = "Error occurred while registering the Category to Database";
        public const string CategoryAlreadyExists = "A category already exists with this name";

        public const string UpdateCategorySuccess = "Category successfully updated";
        public const string UpdateCategoryError = "Error occurred while updating the Category in Database";

        public const string DeleteCategorySuccess = "Category successfully deleted";
        public const string DeleteCategoryError = "Error occurred while deleting the Category from Database";

        public const string EmptyProductListForCategoryError = "This category does not have any products";
        #endregion

        #region SellerApplication Messages 
        public const string ApplicationNotFound = "Application not found";
        public const string EmptySellerApplicationList = "Application list is empty with the current filter";

        public const string CreateSellerApplicationSuccess = "Your application has been successfully received.";
        public const string CreateSellerApplicationError = "An error occurred while creating your application.";
        public const string UpdateSellerApplicationError = "Error occurred while updating the Seller Application in Database";

        public const string ExistingSellerApplicationError = "You already have a pending application.";
        public const string ExistingApprovedSellerApplicationError = "You already approved this application.";

        public const string DeleteSellerApplicationSuccess = "Application successfully deleted";
        public const string DeleteSellerApplicationError = "Error occurred while deleting the Application from Database";
        public const string ApprovedApplicationSuccess = "Application is approved and seller is created successfully";
        public const string ApprovedApplicationError = "Error occurred while approving application";
        public const string RejectedApplicationSuccess = "Application is rejected successfully";
        public const string RejectedApplicationError = "Error occurred while rejecting application";
        public const string ExistingRejectedSellerApplicationError = "You already rejected this application.";
        #endregion

        #region User
        public const string UserNotFound = "User not found";
        public const string CreateUserError = "Error occurred while creating user";
        public const string UserRoleError = "Error occurred while adding 'Seller' role to user";
        #endregion

        #region Email
        public const string ErrorSentEmail = "The application was approved but the e-mail could not be sent.";
        #endregion

        #region Shop Messages
        public const string ShopNotFound = "Shop not found";
        public const string EmptyShopList = "No shops available with the current filter";

        public const string CreateShopSuccess = "Shop request submitted. Waiting for admin approval.";
        public const string CreateShopError = "Error occurred while creating the shop.";

        public const string UpdateShopSuccess = "Shop successfully updated.";
        public const string UpdateShopError = "Error occurred while updating the shop.";

        public const string DeleteShopSuccess = "Shop successfully deleted.";
        public const string DeleteShopError = "Error occurred while deleting the shop.";

        public const string ShopAlreadyExists = "A shop with this name already exists.";
        public const string ShopApprovedSuccess = "Shop approved successfully.";
        public const string ShopApprovedError = "Error occurred while approving the shop.";
        public const string ShopRejectedSuccess = "Shop rejected successfully.";
        public const string ShopRejectedError = "Error occurred while rejecting the shop.";
        public const string ExistingApprovedShopError = "This shop has already been approved.";
        public const string ExistingRejectedShopError = "This shop has already been rejected.";
        public const string ShopStatusPending = "Your shop request is under review.";
        public const string ShopStatusApproved = "Your shop is active and visible to customers.";
        public const string ShopStatusRejected = "Your shop request has been rejected. Please check the details and reapply.";
        #endregion
    }
}
