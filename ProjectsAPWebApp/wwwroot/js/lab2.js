const uri = 'api/Admins';
let admins = [];

function getAdmins() {
    fetch(uri)
        .then(response => response.json())
        .then(data => _displayAdmins(data))
        .catch(error => console.error('Unable to get admins.', error));
}

function addAdmin() {
    const addNameTextbox = document.getElementById('add-name');
    const addCompanyIdTextbox = document.getElementById('add-companyId');

    const admin = {
        name: addNameTextbox.value.trim(),
        companyId: parseInt(addCompanyIdTextbox.value.trim(), 10)
    };

    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(admin)
    })
        .then(response => response.json())
        .then(() => {
            getAdmins();
            addNameTextbox.value = '';
            addCompanyIdTextbox.value = '';
        })
        .catch(error => console.error('Unable to add admin.', error));
}

function deleteAdmin(id) {
    fetch(`${uri}/${id}`, {
        method: 'DELETE'
    })
        .then(() => getAdmins())
        .catch(error => console.error('Unable to delete admin.', error));
}

function displayEditForm(id) {
    const admin = admins.find(admin => admin.id === id);

    document.getElementById('edit-id').value = admin.id;
    document.getElementById('edit-name').value = admin.name;
    document.getElementById('edit-companyId').value = admin.companyId;
    document.getElementById('editForm').style.display = 'block';
}

function updateAdmin() {
    const adminId = document.getElementById('edit-id').value;
    const admin = {
        id: parseInt(adminId, 10),
        name: document.getElementById('edit-name').value.trim(),
        companyId: parseInt(document.getElementById('edit-companyId').value.trim(), 10)
    };

    fetch(`${uri}/${adminId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(admin)
    })
        .then(() => getAdmins())
        .catch(error => console.error('Unable to update admin.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayAdmins(data) {
    const tBody = document.getElementById('admins');
    tBody.innerHTML = '';

    const button = document.createElement('button');

    data.forEach(admin => {
        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${admin.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteAdmin(${admin.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        let textNode = document.createTextNode(admin.name);
        td1.appendChild(textNode);

        let td2 = tr.insertCell(1);
        let textNodeCompanyId = document.createTextNode(admin.companyId);
        td2.appendChild(textNodeCompanyId);

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    admins = data;
}

