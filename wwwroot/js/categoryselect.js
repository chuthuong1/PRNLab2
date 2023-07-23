function onCategoryChange() {
    var selectedCategoryId = document.getElementById("category").value;
    var searchTerm = document.getElementById("searchTerm").value;

    // Gửi yêu cầu lấy sản phẩm theo danh mục và tìm kiếm theo tên đến server
    var url = `/Products/List?categoryId=${selectedCategoryId}&searchTerm=${searchTerm}`;
    window.location.href = url;
}

window.onload = function () {
    var selectedCategoryId = localStorage.getItem("selectedCategoryId");

    // Kiểm tra nếu có giá trị danh mục được lưu trong local storage
    if (selectedCategoryId) {
        // Gửi yêu cầu lấy sản phẩm theo danh mục và tìm kiếm theo tên đến server
        var url = `/Products/List?categoryId=${selectedCategoryId}&searchTerm=${searchTerm}`;
        window.location.href = url;

        // Xóa giá trị danh mục đã lưu trong local storage
        localStorage.removeItem("selectedCategoryId");
    }
};