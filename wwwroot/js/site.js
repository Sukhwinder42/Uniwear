$(document).ready(function () {

    $("#searchBox").keyup(function () {

        var query = $(this).val();

        if (query.length > 0) {

            $.ajax({
                url: "/Product/SearchSuggestions",
                type: "GET",
                data: { term: query },
                success: function (data) {

                    $("#searchResults").empty();

                    data.forEach(function (item) {

                        $("#searchResults").append(
                            `<a href="/Product/Details/${item.id}"
                               class="list-group-item list-group-item-action">
                                ${item.name}
                             </a>`
                        );

                    });

                }
            });

        } else {
            $("#searchResults").empty();
        }

    });

});

//document.addEventListener("DOMContentLoaded", function () {

//    function applyFilters() {
//        let selectedCategories = [];

//        document.querySelectorAll('.category-filter:checked').forEach(cb => {
//            selectedCategories.push(cb.value);
//        });

//        let maxPrice = document.getElementById("priceRange").value;

//        fetch(`/Product/Filter?categoryIds=${selectedCategories.join(',')}&maxPrice=${maxPrice}`)
//            .then(res => res.text())
//            .then(data => {
//                document.getElementById("productContainer").innerHTML = data;
//            });
//    }

//    document.querySelectorAll('.category-filter').forEach(cb => {
//        cb.addEventListener('change', applyFilters);
//    });

//    const priceRange = document.getElementById("priceRange");
//    const priceValue = document.getElementById("priceValue");

//    priceRange.addEventListener("input", function () {
//        priceValue.innerText = this.value;
//        applyFilters();
//    });

//});

//document.addEventListener("DOMContentLoaded", function () {

//    function applyFilters() {

//        let selectedCategories = [];

//        document.querySelectorAll('.category-filter:checked').forEach(cb => {
//            selectedCategories.push(cb.value);
//        });

//        let maxPrice = document.getElementById("priceRange").value;

//        let sort = document.getElementById("sortOption").value;

//        fetch(`/Product/Filter?categoryIds=${selectedCategories.join(',')}&maxPrice=${maxPrice}&sort=${sort}`)
//            .then(res => res.text())
//            .then(data => {
//                document.getElementById("productContainer").innerHTML = data;
//            });
//    }

//    // Category
//    document.querySelectorAll('.category-filter').forEach(cb => {
//        cb.addEventListener('change', applyFilters);
//    });

//    // Price
//    const priceRange = document.getElementById("priceRange");
//    const priceValue = document.getElementById("priceValue");

//    priceRange.addEventListener("input", function () {
//        priceValue.innerText = this.value;
//        applyFilters();
//    });

//    // SORT CHANGE
//    document.getElementById("sortOption")
//        .addEventListener("change", applyFilters);

//});

document.addEventListener("DOMContentLoaded", function () {

    let selectedCategory = 0;

    document.querySelectorAll(".category-btn").forEach(btn => {
        btn.addEventListener("click", function () {
            selectedCategory = this.dataset.id;
            loadProducts();
        });
    });

    const priceRange = document.getElementById("priceRange");
    const priceValue = document.getElementById("priceValue");

    priceRange.addEventListener("input", function () {
        priceValue.innerText = this.value;
    });

    document.getElementById("applyFilter").addEventListener("click", function () {
        loadProducts();
    });

    document.getElementById("sortOption").addEventListener("change", function () {
        loadProducts();
    });

    function loadProducts() {

        let price = document.getElementById("priceRange").value;
        let sort = document.getElementById("sortOption").value;

        fetch(`/Product/Filter?categoryId=${selectedCategory}&maxPrice=${price}&sort=${sort}`)
            .then(res => res.text())
            .then(data => {
                document.getElementById("productContainer").innerHTML = data;
            });
    }

});

