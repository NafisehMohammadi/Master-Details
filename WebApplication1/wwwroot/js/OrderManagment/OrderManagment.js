const baseUrl = "/Order";

const customerApiUrl = "/Customer/GetAll";
const productApiUrl = "/Product/GetAll";

let orderDetails = [];

// =============================
// Get Customers
// =============================
async function getCustomersApi() {

    const response = await fetch(customerApiUrl, {

        method: "GET",

        headers: {
            "Accept": "application/json"
        }

    });


    if (!response.ok) {

        throw new Error(
            "Customer API Error : " + response.status
        );

    }


    return await response.json();
}



// =============================
// Get Products
// =============================
async function getProductsApi() {

    const response = await fetch(productApiUrl, {

        method: "GET",

        headers: {
            "Accept": "application/json"
        }

    });


    if (!response.ok) {

        throw new Error(
            "Product API Error : " + response.status
        );

    }


    return await response.json();
}

// =============================
// Fill Customer Select
// =============================

async function loadCustomers() {

    const customers =
        await loadCustomersService();


    let html = `

    <option value="">
        -- Select Customer --
    </option>

    `;


    customers.forEach(c => {


        html += `

        <option value="${c.id}">
            ${c.firstName} ${c.lastName}
        </option>

        `;


    });


    document.getElementById(
        "editCustomerId"
    ).innerHTML = html;

}




// =============================
// Fill Product Select
// =============================

async function loadProducts() {


    const products =
        await loadProductsService();


    let html = `

    <option value="">
        -- Select Product --
    </option>

    `;



    products.forEach(p => {


        html += `

        <option value="${p.id}"
                data-price="${p.unitPrice}">

            ${p.title}

        </option>

        `;


    });



    document.getElementById(
        "productSelect"
    ).innerHTML = html;

}

//Create Order
async function createOrder() {

    if (orderDetails.length === 0) {

        alert("Please add at least one product.");

        return;
    }

    const order = {

        customerId:
            document.getElementById("editCustomerId").value,

        orderDate:
            document.getElementById("editOrderDate").value,

        description:
            document.getElementById("editDescription").value,

        // details: orderDetails


        details: orderDetails.map(d => ({

            id: d.id,

            productId: d.productId,

            quantity: Number(d.quantity),

            unitPrice: Number(d.unitPrice),

            //lineTotal: Number(d.lineTotal)

        }))
    };


    try {
        console.log(
            "Sending Order:",
            JSON.stringify(order, null, 2)
        );

        const response = await fetch(`${baseUrl}/PostOrderAsync`, {

            method: "POST",

            headers: {

                "Content-Type": "application/json",

                "Accept": "application/json"
            },

            body: JSON.stringify(order)

        });


        if (!response.ok) {

            throw new Error(
                "HTTP Error : " + response.status
            );
        }


        const result = await response.json();


        if (result.isSuccess) {

            alert("Created Successfully");

            resetForm();

            await loadOrder();

        }
        else {

            alert(result.message);
        }


    }
    catch (error) {

        console.error(error);

        alert(error.message);
    }
}


//GetAll Orders (Fetch)

async function loadOrder() {

    try {

        const response = await fetch(`${baseUrl}/GetAll`, {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        });

      /*  console.log(response.status);
        console.log(response.statusText);
        console.log(response);*/

        if (!response.ok) {
            throw new Error(
                "HTTP Error: " + response.status
            );
        }


        const result = await response.json();


        let html = "";


        const orders = result.data ?? [];


        orders.forEach(o => {

            html += `
            <tr>

                <td>${o.orderNumber}</td>

               <td>${o.customerName}</td> 
          
                <td>
                    ${new Date(o.orderDate)
                    .toLocaleDateString()}
                </td>

                <td>${o.totalAmount}</td>

                <td>

                    <button onclick="editOrder('${o.id}')">
                        Edit
                    </button>

                    <button onclick="deleteOrder('${o.id}')">
                        Delete
                    </button>

                </td>

            </tr>
            `;

        });


        document.getElementById("ordersTable")
            .innerHTML = html;


    }
    catch (error) {
        console.error(error);
        alert(error.message);
    }
}

//GetById

async function getOrderById(id) {

    const response = await fetch(`${baseUrl}/GetById?id=${id}`, {

        method: "GET",

        headers: {
            "Accept": "application/json"
        }
    });


    if (!response.ok) {

        throw new Error(
            "HTTP Error : " + response.status
        );
    }


    return await response.json();
}

//Preparing the edit form

async function editOrder(id) {

    try {

        const result = await getOrderById(id);


        if (!result.isSuccess) {

            alert(result.message);

            return;
        }


        const data = result.data;


        // Header

        document.getElementById("editOrderId").value =
            data.id;


        document.getElementById("editCustomerId").value =
            data.customerId;


        document.getElementById("editOrderDate").value =
            data.orderDate.split("T")[0];


        document.getElementById("editDescription").value =
            data.description;



        // Details

        orderDetails = [];


        data.details.forEach(d => {

            orderDetails.push({

                id: d.id,

                productId: d.productId,

                productTitle: d.productTitle,

                quantity: d.quantity,

                unitPrice: d.unitPrice,

                lineTotal:
                    d.quantity * d.unitPrice
            });

        });



        renderDetails();

        calculateTotalAmount();



        // UI

        document.getElementById("formTitle")
            .innerText = "Edit Order";


        document.getElementById("saveButton")
            .innerText = "Update";


        document.getElementById("editPanel")
            .style.display = "block";


    }
    catch (error) {

        console.error(error);

        alert(error.message);
    }
}


//Update Order
async function updateOrder() {

    try {

        const order = {

            id: document.getElementById("editOrderId").value,

            customerId:
                document.getElementById("editCustomerId").value,

            orderDate:
                document.getElementById("editOrderDate").value,

            description:
                document.getElementById("editDescription").value,


            details: orderDetails.map(d => ({

                id: d.id,

                productId: d.productId,

                quantity: Number(d.quantity),

                unitPrice: Number(d.unitPrice),

             /*   lineTotal:
                    Number(d.quantity) *
                    Number(d.unitPrice)*/

            }))
        };


        console.log(
            "Update Order:",
            JSON.stringify(order, null, 2)
        );


        const response = await fetch(
            `${baseUrl}/PutOrderAsync`,
            {
                method: "PUT",

                headers: {
                    "Content-Type": "application/json",
                    "Accept": "application/json"
                },

                body: JSON.stringify(order)
            }
        );


        const result =
            await response.json();


        if (result.isSuccess) {

            alert("Updated Successfully");

            document.getElementById("editPanel")
                .style.display = "none";

            await loadOrder();

        }
        else {

            alert(result.message);

        }

    }
    catch (error) {

        console.error(error);

        alert(error.message);
    }
}
//Delete Order
async function deleteOrder(id) {

    try {

        // 1. Confirm
        const confirmDelete =
            confirm("Are you sure you want to delete this order?");

        if (!confirmDelete) return;

        // 2. DTO
        const dto = { id: id };

        // 3. Request
        const response = await fetch(`${baseUrl}/DeleteAsync`, {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            },
            body: JSON.stringify(dto)
        });

        if (!response.ok) {
            throw new Error("HTTP Error: " + response.status);
        }

        const result = await response.json();

        // 4. Check result
        if (result.isSuccess) {

            alert("Deleted Successfully");

            // 5. Refresh table
            await loadOrder();
        }
        else {
            alert(result.message);
        }

    }
    catch (error) {
        console.error(error);
        alert(error.message);
    }
}
function renderDetails() {

    let html = "";

    orderDetails.forEach(d => {

        html += `
        <tr>

            <td>
                ${d.productTitle}

                <input type="hidden"
                       class="productId"
                       value="${d.productId}" />
            </td>


            <td>
                <input type="number"
                         class="quantity form-control"
                          value="${d.quantity}"
                          onchange="detailChanged('${d.id}', this)" />            </td>


            <td>
              <input type="number"
                           class="unitPrice form-control"
                           value="${d.unitPrice}"
                           onchange="detailChanged('${d.id}', this)" />
            </td>


            <td>
                ${d.lineTotal.toFixed(2)}
            </td>


            <td>
                <button class="btn btn-danger"
                        onclick="removeDetail('${d.id}')">

                    Delete

                </button>
            </td>

        </tr>
        `;

    });


    document.getElementById("tblDetailsBody")
        .innerHTML = html;
}
function detailChanged(id, element) {


    const detail =
        orderDetails.find(x => x.id === id);


    if (!detail)
        return;


    const row =
        element.closest("tr");


    detail.quantity =
        Number(row.querySelector(".quantity").value);


    detail.unitPrice =
        Number(row.querySelector(".unitPrice").value);


    detail.lineTotal =
        detail.quantity * detail.unitPrice;


    renderDetails();

    calculateTotalAmount();
}
function addDetail() {

    const productSelect =
        document.getElementById("productSelect");

    const productId =
        productSelect.value;

    if (productId === "") {

        alert("Please select product.");

        return;
    }

    const productTitle =
        productSelect.options[
            productSelect.selectedIndex
        ].text;

    const quantity =
        Number(document.getElementById("qty").value);

    const unitPrice =
        Number(document.getElementById("unitPrice").value);

    if (quantity <= 0) {

        alert("Quantity is invalid.");

        return;
    }

    if (unitPrice <= 0) {

        alert("Price is invalid.");

        return;
    }

    const detail = {

        id: crypto.randomUUID(),

        productId: productId,

        productTitle: productTitle,

        quantity: quantity,

        unitPrice: unitPrice,

        lineTotal: quantity * unitPrice
    };

    orderDetails.push(detail);

    renderDetails();

    calculateTotalAmount();

    clearDetailInputs();
}
function removeDetail(id) {

    orderDetails =
        orderDetails.filter(x => x.id !== id);

    renderDetails();

    calculateTotalAmount();
}
function clearDetailInputs() {

    document.getElementById(
        "productSelect"
    ).selectedIndex = 0;

    document.getElementById(
        "qty"
    ).value = "";

    document.getElementById(
        "unitPrice"
    ).value = "";
}
function calculateTotalAmount() {

    let total = 0;

    orderDetails.forEach(d => {

        total += d.lineTotal;

    });

    document.getElementById("orderTotal").innerText =
        total.toFixed(2);
}
function initializeProductEvents() {

    document.getElementById("productSelect")
        .addEventListener("change", function () {

            const selected = this.options[this.selectedIndex];

            document.getElementById("unitPrice").value =
                selected.dataset.price ?? "";

        });

}
function resetForm() {

    document.getElementById("editOrderId").value = "";

    document.getElementById("editCustomerId").value = "";

    document.getElementById("editOrderDate").value = "";

    document.getElementById("editDescription").value = "";

    // document.getElementById("tblDetailsBody").innerHTML = "";

    orderDetails = [];

    renderDetails();
}
function newOrder() {

    resetForm();

    document.getElementById("formTitle").innerText = "Create Order";

    document.getElementById("saveButton").innerText = "Create";

    document.getElementById("editOrderDate").value =  new Date().toISOString().split("T")[0];

    document.getElementById("editPanel").style.display = "block";
}
function saveOrder() {

    const id = document.getElementById("editOrderId").value;


    if (id === "") {

        createOrder();

    }
    else {

        updateOrder();

    }

}
async function loadCustomersService() {

    const result = await getCustomersApi();

    return result.data ?? result;
}



// =============================
// Product Service
// =============================

async function loadProductsService() {

    const result = await getProductsApi();

    return result.data ?? result;
}