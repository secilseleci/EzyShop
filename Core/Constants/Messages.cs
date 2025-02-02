using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Constants
{
    public class Messages
    {
        #region Product Messages 
        public const string ProductNotFound = "Product not found";
        public const string EmptyProductList = "Product list is empty with the current filter";

        public const string CreateProductSuccess = "Product created successfully";
        public const string CreateProductError = "Error occured while registering the Product to Database";

        public const string UpdateProductSuccess = "Product successfully updated";
        public const string UpdateProductError = "Error occured while updating the Product in Database";

        public const string DeleteProductSuccess = "Product successfully deleted";
        public const string DeleteProductError = "Error occured while deleting the Product from Database";
        #endregion

        #region Category Messages
        public const string CategoryNotFound = "Category not found";
        public const string EmptyCategoryList = "Category list is empty with the current filter";

        public const string CreateCategorySuccess = "Category created successfully";
        public const string CreateCategoryError = "Error occured while registering the Category to Database";
        public const string CategoryAlreadyExists = "A category already exists with this name";

        public const string UpdateCategorySuccess = "Category successfully updated";
        public const string UpdateCategoryError = "Error occured while updating the Category in Database";

        public const string DeleteCategorySuccess = "Category successfully deleted";
        public const string DeleteCategoryError = "Error occured while deleting the Category from Database";

        public const string EmptyProductListForCategoryError = "This category does not have any products";




        #endregion

        #region SellerApplication Messages 
        public const string ApplicationNotFound = "Application not found";
        public const string EmptySellerApplicationList = "Application list is empty with the current filter";
        
        public const string CreateSellerApplicationSuccess = "Your application has been successfully received.";
        public const string CreateSellerApplicationError = "An error occurred while creating your application.";
        public const string UpdateSellerApplicationError = "Error occured while updating the Seller Application in Database";

        public const string ExistingSellerApplicationtError = "You already have a pending application.";
        public const string ExistingApprovedSellerApplicationtError = "You already approved this application.";

        public const string DeleteSellerApplicationSuccess = "Application successfully deleted";
        public const string DeleteSellerApplicationError = "Error occured while deleting the Application from Database";
        public const string ApprovedApplicationSuccess = "Application is approved and seller is created successfully";
        public const string ApprovedApplicationError = "Error occured while approving application";
        #endregion
        #region User
        public const string UserNotFound = "User not found";
        public const string CreateUserError = "Error occured while creating user";
        public const string UserRoleError = "Error occured while adding 'Seller' role to user";
        #endregion
        #region Email
        public const string ErrorSentEmail= "The application was approved but the e-mail could not be sent.";
       
        #endregion
    }
}
