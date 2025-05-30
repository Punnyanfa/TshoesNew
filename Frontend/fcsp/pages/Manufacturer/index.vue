<template>
  <div>
    <HeaderManu @logout="logout" />
    <div class="manufacturer-layout">
      <div class="main-content">
        <div class="container-fluid mt-4">
          <!-- Profile Section -->
          <div class="card mb-4">
            <div class="card-header" style="background-color: #AAAAAA; color: white;">
              <h4>Hồ sơ nhà sản xuất</h4>
            </div>
            <div class="card-body">
              <form @submit.prevent="saveProfile">
                <div class="row mb-3">
                  <div class="col-md-6">
                    <div class="form-group mb-3">
                      <label for="manufacturerName">Tên công ty</label>
                      <input 
                        type="text" 
                        class="form-control" 
                        id="manufacturerName" 
                        v-model="profile.name"
                        placeholder="Nhập tên công ty"
                        required
                      >
                    </div>
                    
                    <div class="form-group mb-3">
                      <label for="contactPerson">Tên người liên lạc</label>
                      <input 
                        type="text" 
                        class="form-control" 
                        id="contactPerson" 
                        v-model="profile.contactPerson"
                        placeholder="Nhập tên người liên lạc"
                        required
                      >
                    </div>
                    
                    <div class="form-group mb-3">
                      <label for="email">Email</label>
                      <input 
                        type="email" 
                        class="form-control" 
                        id="email" 
                        v-model="profile.email"
                        placeholder="Nhập địa chỉ email"
                        required
                      >
                    </div>
                  </div>
                  
                  <div class="col-md-6">
                    <div class="form-group mb-3">
                      <label for="phone">Số điện thoại</label>
                      <input 
                        type="tel" 
                        class="form-control" 
                        id="phone" 
                        v-model="profile.phone"
                        placeholder="Nhập số điện thoại"
                        required
                      >
                    </div>
                    
                    <div class="form-group mb-3">
                      <label for="address">Địa chỉ</label>
                      <textarea 
                        class="form-control" 
                        id="address" 
                        v-model="profile.address"
                        placeholder="Nhập địa chỉ"
                        rows="2"
                        required
                      ></textarea>
                    </div>
                  </div>
                </div>
                
                <div class="d-flex justify-content-end">
                  <button type="submit" class="btn" style="background-color: #AAAAAA; color: white; border: none;">Lưu hồ sơ</button>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import HeaderManu from '@/components/HeaderManu.vue';

export default {
  name: 'ManufacturerHome',
  components: { HeaderManu },
setup() {
  if (typeof window !== 'undefined') {
    const token = localStorage.getItem('userToken');
    const role = localStorage.getItem('role');
    console.log('ManufacturerHome - Token:', token, 'Role:', role); // Debugging log
    if (!token) {
      console.warn('No user token found. Redirecting to login page.');
      alert('Please log in to access this page.');
      window.location.href = '/loginPage';
      return;
    }
    if (!role || role.toLowerCase() !== 'manufacturer') {
      console.warn('User role is not Manufacturer. Role found:', role);
      alert('Access denied: You need a Manufacturer role to view this page.');
      window.location.href = '/loginPage';
    }
  }
  return {};
},
  data() {
    return {
      profile: {
        name: '',
        contactPerson: '',
        email: '',
        phone: '',
        address: ''
      }
    };
  },
  methods: {
    saveProfile() {
      console.log('Saving profile:', this.profile);
    },
    logout() {
      // Clear all localStorage items
      if (typeof window !== 'undefined') {
        localStorage.removeItem('userToken');
        localStorage.removeItem('userEmail');
        localStorage.removeItem('role');
        localStorage.removeItem('userId');
        localStorage.removeItem('userName');
        localStorage.removeItem('userRole');
        localStorage.removeItem('username');
        localStorage.removeItem('ManufacturerId');
      }

      // Redirect to login page
      if (typeof window !== 'undefined') {
        window.location.href = '/loginPage';
      }
    }
  }
};
</script>

<style>
.manufacturer-layout {
  min-height: 100vh;
  background-color: #f8f9fa;
}

.main-content {
  padding: 2rem;
}

.card {
  border: none;
  box-shadow: 0 0 15px rgba(0,0,0,0.1);
  border-radius: 10px;
}

.card-header {
  border-radius: 10px 10px 0 0 !important;
}

.form-control {
  border-radius: 8px;
  border: 1px solid #dee2e6;
  padding: 0.75rem;
}

.form-control:focus {
  border-color: #AAAAAA;
  box-shadow: 0 0 0 0.2rem rgba(170, 170, 170, 0.25);
}

.btn {
  padding: 0.75rem 1.5rem;
  border-radius: 8px;
  font-weight: 500;
  transition: all 0.3s ease;
}

.btn:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 15px rgba(0,0,0,0.1);
}
</style>