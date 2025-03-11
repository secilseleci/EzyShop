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

        public const string ShopAlreadyExists = "You can only have one shop. Please update your existing shop instead.";
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

        #region ShoppingCart Messages 
        public const string ShoppingCartNotFound = "ShoppingCart not found";
        public const string ShoppingCartEmpty = "ShoppingCart is empty";
        public const string ShoppingCartIsAlreadyEmpty= "ShoppingCart is already empty";
        public const string CreateShoppingCartSuccess = "ShoppingCart created successfully";
        public const string CreateShoppingCartError = "Error occurred while registering the ShoppingCart to Database";
 
        public const string DeleteShoppingCartSuccess = "ShoppingCart successfully deleted";
        public const string DeleteShoppingCartError = "Error occurred while deleting the ShoppingCart from Database";
        public const string ClearCartSuccess = "Your cart has been cleared successfully";
        public const string ClearCartError = "An error occurred while emptying the cart";
 
        #endregion

        #region ShoppingCartItem Messages 
        public const string ShoppingCartItemNotFound = "ShoppingCartItem not found";

        public const string AddShoppingCartItemSuccess = "ShoppingCartItem has been successfully added to the cart";
        public const string AddShoppingCartItemError = "Error occurred while while registering the ShoppingCartItem to  Database";
        public const string CreateShoppingCartItemError = "Error occurred while registering the ShoppingCartItem to Database";

        public const string DeleteShoppingCartItemSuccess = "ShoppingCart has been successfully deleted";
        public const string DeleteShoppingCartItemError = "Error occurred while deleting the ShoppingCartItem from Database";

        public const string RemoveMultipleItemsFromCartSuccess = "The selected items have been successfully removed from your cart.";
        public const string RemoveMultipleItemsFromCartError = "Removing products failed";
        public const string CartItemCountError = "The number can be minimum 1 and maximum 100.";
        public const string ProductIsNotInYourCart = "This product is not in your cart";

        public const string ChangeCountSuccess = "Quantity has been changed";
        public const string ChangeCountError = "An error occurred while changing the product quantity";

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
    }
}
