//=====================================================
// Order Management
//=====================================================

const baseUrl = "/Order";

const customerApiUrl = "/Customer/GetAll";

const productApiUrl = "/Product/GetAll";

let orderDetails = [];
let btnadddetail = false;

let currentGuidKey = null;
//Initialization
$(document).ready(function () {

    initialize();

});

async function initialize() {

    await loadCustomers();

    await loadProducts();

    await loadOrder();

    initializeProductEvents();

}
//Customer API
async function getCustomersApi() {

    const response = await fetch(customerApiUrl, {

        method: "GET",

        headers: {

            "Accept": "application/json"

        }

    });
  

    if (!response.ok)
        throw new Error("Customer API Error");

    return await response.json();

}
//Product API
async function getProductsApi() {

    const response = await fetch(productApiUrl, {

        method: "GET",

        headers: {

            "Accept": "application/json"

        }

    });

    if (!response.ok)
        throw new Error("Product API Error");

    return await response.json();

}

//=====================================================
//Customer Service
//=====================================================
async function loadCustomersService() {

    const result = await getCustomersApi();

    return result.data ?? [];

}

//=====================================================
//Product Service
//=====================================================
async function loadProductsService() {

    const result = await getProductsApi();

    return result.data ?? [];

}

//=====================================================
//Load Customers
//=====================================================
async function loadCustomers() {

    const customers =
        await loadCustomersService();

    let html = `

<option value="">

Select Customer

</option>

`;

    customers.forEach(c => {

        html += `

<option value="${c.id}">

${c.firstName} ${c.lastName}

</option>

`;

    });

    $("#editCustomerId").html(html);

}

//=====================================================
//Load Products
//=====================================================
async function loadProducts() {

    const products =
        await loadProductsService();

    let html = `

<option value="">

Select Product

</option>

`;

    products.forEach(p => {

        html += `

<option
value="${p.id}"
data-price="${p.unitPrice}">

${p.title}

</option>

`;

    });

    $("#productSelect").html(html);

}
//=====================================================
// Load All Orders
//=====================================================
async function loadOrder() {

    try {

        const response =
            await fetch(`${baseUrl}/GetAll`, {

                method: "GET",

                headers: {
                    "Accept": "application/json"
                }
            });


        if (!response.ok) {

            throw new Error(
                "Load Orders Error : " + response.status
            );

        }


        const result =
            await response.json();

      //  alert(result.data);



        let html = "";

        const orders =
            result.data ?? [];


        orders.forEach(order => {
    

            html += `

            <tr>

                <td>
                    ${order.orderNumber}
                </td>


                <td>
                    ${order.customerName}
                </td>


                <td>
                    ${new Date(order.orderDate)
                    .toLocaleDateString()}
                </td>


                <td>
                    ${order.totalAmount.toFixed(2)}
                </td>


                <td>

                    <button 
                    class="btn btn-warning btn-sm"
                    onclick="editOrder('${order.id}')"  >
                    Edit

                    </button>

                    

                    <button
                    class="btn btn-danger btn-sm"
                    onclick="deleteOrder('${order.id}')">

                    Delete

                    </button>


                </td>


            </tr>

            `;

        });



        document.getElementById(
            "ordersTable"
        ).innerHTML = html;


    }
    catch (error) {

        console.error(error);

        alert(error.message);

    }

}



//=====================================================
//initializeProductEvents
//=====================================================
function initializeProductEvents() {

    document.getElementById("productSelect")
        .addEventListener("change", function () {

            const selected =
                this.options[this.selectedIndex];

            document.getElementById("unitPrice").value =
                selected.dataset.price ?? "";

        });

}
//=====================================================
// newOrder
//=====================================================
function newOrder(){


    resetForm();



    document.getElementById(
        "formTitle"
    ).innerText =
        "New Order";



    document.getElementById(
        "saveButton"
    ).innerText =
        "Create";



    document.getElementById(
        "editOrderDate"
    ).value =
        new Date()
        .toISOString()
        .split("T")[0];



    openOrderModal();

    clearDetailInputs();

}

//=====================================================
// Add Detail
//=====================================================
function addDetail() {


    const productSelect =
        document.getElementById(
            "productSelect"
        );


    const productId =
        productSelect.value;



    if(productId === ""){

        alert("Please select product.");

        return;
    }



    // جلوگیری از محصول تکراری

    const exists =
        orderDetails.some(x =>
            x.productId === productId
        );



    if(exists){

        alert(
        "This product already exists in order."
        );

        return;
    }

    const productTitle =
        productSelect.options[
            productSelect.selectedIndex
        ].text;


    const quantity =
        Number(
            document.getElementById("qty").value
        );


    const unitPrice =
        Number(
            document.getElementById("unitPrice").value
        );


    if(quantity <= 0){

        alert(
        "Quantity is invalid."
        );

        return;

    }

    if(unitPrice <= 0){

        alert(
        "Price is invalid."
        );

        return;

    }


    const detail = {


        //----use tempId----
       // tempId: Date.now(),
       //-------------
        id: null,

        guidKey: currentGuidKey,

        productId:  productId,

        productTitle: productTitle,

        quantity: quantity,

        unitPrice: unitPrice,

        lineTotal:
        quantity * unitPrice

    };
    orderDetails.push(detail);


    renderDetails();

    calculateTotalAmount();

    clearDetailInputs();

}

//=====================================================
// Create Order
//=====================================================
async function createOrder() {


    if (orderDetails.length === 0) {

        alert("Please add at least one product.");

        return;
    }



    // GuidKey فقط هنگام ثبت ساخته می شود

    const guidKey =
        crypto.randomUUID();




    const order = {


        guidKey: guidKey,


        customerId:
            document.getElementById(
                "editCustomerId"
            ).value,


        orderDate:
            document.getElementById(
                "editOrderDate"
            ).value,


        description:
            document.getElementById(
                "editDescription"
            ).value,



        details:

        orderDetails.map(d => ({


            guidKey: currentGuidKey,


            productId:
                d.productId,


            quantity:
                Number(d.quantity),


            unitPrice:
                Number(d.unitPrice)


        }))


    };



    try {

//=====================================================
// Send Create Order To API
//=====================================================

        console.log(
            "Sending Order:",
            JSON.stringify(order,null,2)
        );



        const response =
            await fetch(
                `${baseUrl}/PostOrderAsync`,
                {


                    method:"POST",


                    headers:{


                        "Content-Type":
                        "application/json",


                        "Accept":
                        "application/json"

                    },


                    body:
                    JSON.stringify(order)


                });



        if(!response.ok){


            throw new Error(
                "HTTP Error : "
                + response.status
            );

        }




        const result =
            await response.json();



        if(result.isSuccess){


            alert(
            "Created Successfully"
            );

             closeOrderModal();

            //Reset
            resetForm();

            //clearModal&closeModal

            cancelOrder();

            // Refresh 

            await loadOrder();


        }
        else{


            alert(result.message);


        }



    }
    catch(error){


        console.error(error);


        alert(error.message);


    }


}


//=====================================================
// Get By Id
//=====================================================
async function getOrderById(id) {


    const response =
        await fetch(
        `${baseUrl}/GetById?id=${id}`,
        {

            method:"GET",

            headers:{
                "Accept":"application/json"
            }

        });



    if(!response.ok){

        throw new Error(
            "GetById Error : "
            + response.status
        );

    }
    

    return await response.json();;
 


}

//=====================================================
// Edit Order
//=====================================================

async function editOrder(id) {

    try {
 

        const result =
            await getOrderById(id);
        //console.log(result.data);
       
        console.log("Full Response:", result);

        if (!result.isSuccess) {

            alert(result.message);

            return;
        }

        const order = result.data;

       // console.log("Order:", order);

        // =========================
        // Header
        // =========================
        document.getElementById(
            "editOrderId"
        ).value = order.id;

        currentGuidKey = order.guidKey;
      //  console.log("GuidKey:", currentGuidKey);

        document.getElementById(
            "editCustomerId"
        ).value = order.customerId;

      //  console.log("GuidKey:", currentGuidKey);

        document.getElementById(
            "editOrderDate"
        ).value =
            order.orderDate
            .split("T")[0];


        document.getElementById(
            "editDescription"
        ).value =
            order.description ?? "";


      //  console.log("guidkey:" + currentGuidKey);


        // =========================
        // Details
        // =========================

        orderDetails = [];
        order.details.forEach(detail => {

            orderDetails.push({

                //tempId: crypto.randomUUID(),

                id:
                    detail.id,

                guidKey:
                    detail.guidKey,

                productId:
                    detail.productId,

                productTitle:
                    detail.productTitle,

                quantity:
                    detail.quantity,

                unitPrice:
                    detail.unitPrice,

                lineTotal:
                    detail.lineTotal

            });


        });
        console.log("check orderDetails: "+ orderDetails);


        renderDetails();

        calculateTotalAmount();

        clearDetailInputs();



        // =========================
        // Change Modal State
        // =========================


        document.getElementById(
            "formTitle"
        ).innerText =
            "Edit Order";

        document.getElementById(
            "saveButton"
        ).innerText =
            "Update";



        // Open Bootstrap Modal

        openOrderModal();

     
    }
    catch(error) {


        console.error(error);


        alert(error.message);

    }

}

//=====================================================
// Update Order
//=====================================================
async function updateOrder() {

    const productId =
        document.getElementById("productSelect").value;

    const qty =
        document.getElementById("qty").value;

    const unitPrice =
        document.getElementById("unitPrice").value;

    if (productId || qty || unitPrice) {

        alert(" Add First, click the Add button or clear the information in the new row..");

        return;
    }

    if (orderDetails.length === 0) {

        alert("Order must have at least one detail.");

        return;
    }


    const order = {

        id:
            document.getElementById("editOrderId").value,

        customerId:
            document.getElementById("editCustomerId").value,

        orderDate:
            document.getElementById("editOrderDate").value,

        description:
            document.getElementById("editDescription").value,

        guidKey:
            currentGuidKey,

        details:

            orderDetails.map(d => ({

                id: d.id ?? null, 
                  //  d.id ?? "00000000-0000-0000-0000-000000000000",


                guidKey:
                    currentGuidKey,

                productId:
                    d.productId,


                quantity:
                    Number(d.quantity),


                unitPrice:
                    Number(d.unitPrice)


            }))

    };


    try {

        console.log(
            "Update JSON:",
            JSON.stringify(order,null,2)
        );

        const response =
            await fetch(
                `${baseUrl}/PutOrderAsync`,
                {

                    method:"PUT",


                    headers:{

                        "Content-Type":
                            "application/json",

                        "Accept":
                            "application/json"
                    },


                    body:
                        JSON.stringify(order)

                }
            );



        if(!response.ok){

            throw new Error(
                "HTTP Error : " + response.status
            );

        }


        const result =
            await response.json();

        console.log("update check id: " + order.id);


        if(result.isSuccess){


            alert(
                "Updated Successfully"
            );


            resetForm();

            closeOrderModal();

            clearDetailInputs();

            await loadOrder();


        }
        else{


            alert(result.message);

        }


    }
    catch(error){


        console.error(error);


        alert(error.message);

    }

}

//=====================================================
// Delete Order
//=====================================================
async function deleteOrder(id) {

    alert("idbafter: " + id);

    const confirmDelete =
        confirm(
        "Are you sure you want to delete this order?"
        );


    if(!confirmDelete)
        return;



    try {
  
        //  DTO
        const dto = { id: id };
    
     
        //Request
        const response =
        await fetch(
            `${baseUrl}/DeleteAsync/`,
        {

            method:"DELETE",


            headers:{

                "Content-Type":
                "application/json",

                "Accept":
                "application/json"

            },


           body:
                JSON.stringify(dto)

        });
        alert("idbefore: " + id);


        if(!response.ok){


            throw new Error(
                "Delete Error : "
                + response.status
            );

        }



        const result =
        await response.json();



        if(result.isSuccess){


            alert(
            "Deleted Successfully"
            );


            await loadOrder();


        }
        else{


            alert(result.message);

        }



    }
    catch(error){


        console.error(error);


        alert(error.message);


    }
}

//=====================================================
// Calculate Total Amount
//=====================================================
function calculateTotalAmount() {

    let total = 0;
  console.log(orderDetails);
    orderDetails.forEach(detail => {

        detail.lineTotal =
            Number(detail.quantity) *
            Number(detail.unitPrice);

        total += detail.lineTotal;

    });


    document.getElementById("orderTotal").innerText =
        total.toFixed(2);

}
//=====================================================
// Refresh
//=====================================================

async function refreshOrders(){


    await loadOrder();


}

//=====================================================
// Open Modal
//=====================================================
function openModal(){


    const modal =
    new bootstrap.Modal(

        document.getElementById(
            "orderModal"
        )

    );


    modal.show();


}

//=====================================================
// Close Modal
//=====================================================
function closeModal(){


    const element =
    document.getElementById(
        "orderModal"
    );


    const modal =
    bootstrap.Modal.getInstance(
        element
    );


    if(modal){

        modal.hide();

    }


}

//=====================================================
// Cancel
//=====================================================
function cancelOrder(){


    resetForm();


    closeModal();


}

//=====================================================
// Reset Form
//=====================================================
function resetForm(){



    document.getElementById(
        "editOrderId"
    ).value = "";



    document.getElementById(
        "editCustomerId"
    ).value = "";



    document.getElementById(
        "editOrderDate"
    ).value = "";



    document.getElementById(
        "editDescription"
    ).value = "";



    orderDetails = [];



    renderDetails();

    calculateTotalAmount();

    


}

//=====================================================
// Save
//=====================================================
function saveOrder(){


    const id =
    document.getElementById(
        "editOrderId"
    ).value;



    if(id === ""){


        createOrder();


    }
    else{


        updateOrder();


    }


}

//=====================================================
//openOrderModal
//=====================================================
function openOrderModal(){


    const modal =
    new bootstrap.Modal(
        document.getElementById("orderModal")
    );


    modal.show();

}

// ======================================
// Close Order Modal
// ======================================
function closeOrderModal() {

    const modalElement =
        document.getElementById("orderModal");


    const modal =
        bootstrap.Modal.getInstance(
            modalElement
        );

    if (modal) {

        modal.hide();
    }

}

//=====================================================
//cancelOrder
//=====================================================
function cancelOrder(){

    resetForm();

    const modal =
        bootstrap.Modal.getInstance(
            document.getElementById("orderModal")
        );


    if(modal){

        modal.hide();

    }

}
//=====================================================
//renderDetails
//=====================================================
function renderDetails() {

    let html = "";
    //-----use Index----
    orderDetails.forEach((d, index) => {

        html += `
        <tr>

            <td>
                ${d.productTitle}
            </td>

            <td>
                <input type="number"
                       class="quantity form-control"
                       value="${d.quantity}"
                       onchange="detailChanged(${index},this)" />
            </td>

            <td>
                <input type="number"
                       class="unitPrice form-control"
                       value="${d.unitPrice}"
                       onchange="detailChanged(${index},this)" />
            </td>

            <td>
                ${Number(d.lineTotal).toFixed(2)}
            </td>

            <td>
                <button
                    class="btn btn-danger btn-sm"
                    onclick="removeDetail(${index})">

                    Delete

                </button>
            </td>

        </tr>
        `;

    });

    document.getElementById("tblDetailsBody").innerHTML = html;
    
//-----use tempid-----  
/*orderDetails.forEach(d => {

        html += `
        <tr>

            <td>
                ${d.productTitle}
            </td>

            <td>
                <input type="number"
                       class="quantity form-control"
                       value="${d.quantity}"
                       onchange="detailChanged('${d.tempId}', this)" />
            </td>

            <td>
                <input type="number"
                       class="unitPrice form-control"
                       value="${d.unitPrice}"
                       onchange="detailChanged('${d.tempId}', this)" />
            </td>

            <td>
                ${Number(d.lineTotal).toFixed(2)}
            </td>

            <td>
                <button
                    class="btn btn-danger btn-sm"
                    onclick="removeDetail('${d.tempId}')">

                    Delete

                </button>
            </td>

        </tr>
        `;

    });
    document.getElementById("tblDetailsBody").innerHTML = html;
    */
}

//=====================================================
//detailChanged
//=====================================================

//----use Index----
function detailChanged(index, element) {

    const detail = orderDetails[index];

    if (!detail)
        return;

    const row = element.closest("tr");

    detail.quantity =
        Number(
            row.querySelector(".quantity").value
        );

    detail.unitPrice =
        Number(
            row.querySelector(".unitPrice").value
        );

    detail.lineTotal =
        detail.quantity *
        detail.unitPrice;

    renderDetails();
    calculateTotalAmount();


}

//----use tempId----
/*function detailChanged(tempId, element) { 

    const row = element.closest("tr");

    const qty =
        Number(row.querySelector(".quantity").value);

    const price =
        Number(row.querySelector(".unitPrice").value);

    const detail =
        orderDetails.find(x => x.tempId === tempId);

    if (!detail)
        return;

    detail.quantity = qty;

    detail.unitPrice = price;

    detail.lineTotal = qty * price;

    renderDetails();

    calculateTotalAmount();
}*/


//=====================================================
//removeDetail
//=====================================================

//----use Index----
function removeDetail(index) {

    orderDetails.splice(index, 1);

    renderDetails();

    calculateTotalAmount();

}
//----use tempId----
/*function removeDetail(tempId) {


    orderDetails =
        orderDetails.filter(
            x => x.tempId !== tempId
        );

    renderDetails();

    calculateTotalAmount();

}
*/
//=====================================================
// Clear Detail Inputs
//=====================================================
function clearDetailInputs() {

    document.getElementById("productSelect").value = "";

    document.getElementById("qty").value = "";

    document.getElementById("unitPrice").value = "";

}