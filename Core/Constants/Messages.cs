namespace Core.Constants
{
    public class Messages
    {
        #region Login / Logout Messages
        public const string LoginSuccess = "Login successful.";
        public const string LoginInvalidCredentials = "Email or password is incorrect.";
        public const string LoginUserNotFound = "User not found.";
        public const string LogoutSuccess = "Logout successful.";
        public const string LoginUnauthorized = "Please login to add items to cart.";

        #endregion

        #region Registration
        public const string RegisterSuccess = "Registration completed successfully.";
        #endregion

        #region Password
        public const string PasswordChangeSuccess = "Password changed successfully.";
        public const string OldPasswordError = "Old password is wrong";
        #endregion


        #region Product Messages 
        public const string ProductNotFound = "Product not found";
        public const string EmptyProductList = "Product list is empty with the current filter";

        public const string CreateProductSuccess = "Product created successfully";
        public const string CreateProductError = "Error occurred while registering the Product to Database";

        public const string UpdateProductSuccess = "Product successfully updated";
        public const string UpdateProductError = "Error occurred while updating the Product in Database";

        public const string DeleteProductSuccess = "Product successfully deleted";
        public const string DeleteProductError = "Error occurred while deleting the Product from Database";

        public const string NoProductFilters = "No products found matching the filters";

        public const string ProductAddedSuccess = " Product added to cart";
        public const string ProductAddedError = "Error occurred while adding the Product to Cart";


        public const string InsufficientStock = "Product stock is insufficient";

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
        public const string EmailAlreadyRegistered = "This email is already registered in the system. Please use another email address for your seller application.";

        public const string CreateApplicationSuccess = "Your application has been successfully received";
        public const string CreateApplicationError = "An error occurred while creating your application";
        public const string UpdateApplicationError = "An error occurred while updating your application";

        public const string ApplicationAlreadyExist = "You already have a pending application";
        public const string ApplicationAlreadyApproved = "This application already approved";
        public const string ApplicationAlreadyRejected = "This application already rejected";

        public const string DeleteApplicationSuccess = "Application successfully deleted";
        public const string DeleteApplicationError = "Error occurred while deleting the Application from Database";

        public const string ApprovedApplicationSuccess = "Application is approved and seller is created successfully";
        public const string ApprovedApplicationError = "Error occurred while approving application";

        public const string RejectedApplicationSuccess = "Application is rejected successfully";
        public const string RejectedApplicationError = "Error occurred while rejecting application";

        public const string EmptyApplicationList = "Application list is empty with the current filter";
        #endregion

        #region Customer
        public const string CustomerNotFound = "Customer not found";
        public const string DeleteCustomerSuccess = "Customer successfully deleted";
        public const string DeleteCustomerError = "Error occurred while deleting the Customer from Database";
        public const string EmptyCustomerList = "Customer list is empty with the current filter";

        #endregion


        #region Seller
        public const string CreateSellerError = "An error occurred while creating Seller";
        public const string SellerNotFound = "Seller not found";
        public const string DeleteSellerSuccess = "Seller successfully deleted";
        public const string DeleteSellerError = "Error occurred while deleting the Seller from Database";
        public const string EmptySellerList = "Seller list is empty with the current filter";

        #endregion


        #region User
        public const string UserNotFound = "User not found";
        public const string CreateUserError = "Error occurred while creating user";
        public const string UserRoleError = "Error occurred while adding 'Seller' role to user";
        public const string UnauthorizedAccess = "You do not have authorization to perform this action";
        public const string UserNotAuthenticated = "User not authenticated ";

        #endregion

        #region Email
        public const string ErrorSentEmail = "The application was approved but the e-mail could not be sent.";
        #endregion

        #region Shop Messages
        public const string ShopNotFound = "Shop not found";
        public const string ShopInActive = "Your shop is currently inactive.";
        public const string ShopAlreadyExists = "Shop is already exists.";
        public const string CreateShopSuccess = "Shop request submitted. Waiting for admin approval.";
        public const string CreateShopError = "Error occurred while creating the shop.";

        public const string EmptyShopList = "No shops available with the current filter";
        public const string DeleteShopSuccess = "Shop successfully deleted";
        public const string DeleteShopError = "Error occurred while deleting the Shop from Database";



        #endregion

        #region  Cart Messages 
        public const string CartNotFound = "Cart not found";
        public const string CartEmpty = "Cart is empty";
        public const string CartIsAlreadyEmpty = "Cart is already empty";
        public const string CreateCartSuccess = "Cart created successfully";
        public const string CreateCartError = "Error occurred while registering the Cart to Database";

        public const string DeleteCartSuccess = "Cart successfully deleted";
        public const string DeleteCartError = "Error occurred while deleting the Cart from Database";
        public const string ClearCartSuccess = "Your cart has been cleared successfully";
        public const string ClearCartError = "An error occurred while emptying the cart";

        #endregion

        #region CartLine Messages 
        public const string CartLineNotFound = "CartLine not found";

        public const string CreateCartLineSuccess = "CartLine has been successfully added to the cart";
        public const string CreateCartLineError = "Error occurred while registering the CartLine to Database";

        public const string DeleteCartLineSuccess = "CartLine has been successfully deleted";
        public const string DeleteCartLineError = "Error occurred while deleting the CartLine from Database";

      

        #endregion


        #region Order
        public const string OrderNotFound = "Order not found";
        public const string EmptyOrderList = "Order list is empty";
        public const string CreateOrderSuccess = "Order created successfully";
        public const string CreateOrderError = "Error occurred while registering the Order to Database";
        public const string OrderStatusIsNotPending = "Order is not in Pending status";
        public const string OrderMarkAsShippedSuccess = "Order marked as shipped successfully";
        public const string OrderMarkAsShippedError = "Error occurred while marking the order as shipped";
        public const string CustomerEmailError = "customer e-mail not found";
        #endregion



        public const string NoChangesDetected = "You haven't made any changes";

        #region Configuration
        public const string PasswordError = "Default seller password is not configured. Please contact the administrator.";
        #endregion
    }


}
