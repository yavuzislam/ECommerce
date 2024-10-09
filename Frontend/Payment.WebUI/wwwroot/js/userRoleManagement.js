document.addEventListener('DOMContentLoaded', () => {
    const form = document.querySelector('#roleForm');
    const userIdInput = document.querySelector('input[name="userId"]');
    const rolesCheckboxes = document.querySelectorAll('input[name="roles"]');
    const updateButton = document.querySelector('button[type="submit"]');
    const antiForgeryInput = document.querySelector('input[name="__RequestVerificationToken"]'); // Anti-Forgery Token

    // Set event listener for form submission
    form.addEventListener('submit', (event) => {
        event.preventDefault();
        const userId = userIdInput.value;
        const selectedRoles = Array.from(rolesCheckboxes)
            .filter(checkbox => checkbox.checked)
            .map(checkbox => checkbox.value);

        console.log('Updating roles for User ID:', userId);
        console.log('Selected roles:', selectedRoles);

        // Prepare data for submission
        const formData = new FormData();
        formData.append('__RequestVerificationToken', antiForgeryInput.value);
        formData.append('userId', userId);
        selectedRoles.forEach(role => {
            formData.append('roles', role);
        });

        // Send the updated roles to the server using fetch API without page refresh
        fetch('/Admin/UserRole/UpdateRoles', {
            method: 'POST',
            body: formData
        })
            .then(response => {
                return response.text().then(text => {
                    console.log('Response Text:', text);
                    if (response.ok) {
                        try {
                            const jsonData = JSON.parse(text);
                            return jsonData;
                        } catch (e) {
                            throw new Error('Geçersiz JSON yanıtı');
                        }
                    } else {
                        throw new Error('Failed to update roles: ' + response.statusText);
                    }
                });
            })
            .then(result => {
                console.log('Roles updated successfully:', result);
                alert('Rolleri başarıyla güncellendi');
            })
            .catch(error => {
                console.error('Error updating roles:', error);
                alert('Rolleri güncellerken bir hata oluştu: ' + error.message);
            });
    });

    // Set event listener for role checkboxes
    rolesCheckboxes.forEach(checkbox => {
        checkbox.addEventListener('change', () => {
            console.log('Role', checkbox.value, 'checked:', checkbox.checked);
        });
    });
});
