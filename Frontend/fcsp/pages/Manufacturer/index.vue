<template>
  <div>
    <HeaderManu @logout="logout" />
    <div class="manufacturer-layout">
      <div class="main-content">
        <div class="container-fluid mt-4">
          <!-- Profile Section -->
          <div class="card mb-4">
            <div class="card-header bg-primary text-white">
              <h4>Manufacturer Profile</h4>
            </div>
            <div class="card-body">
              <form @submit.prevent="saveProfile">
                <div class="row mb-3">
                  <div class="col-md-6">
                    <div class="form-group mb-3">
                      <label for="manufacturerName">Company Name</label>
                      <input 
                        type="text" 
                        class="form-control" 
                        id="manufacturerName" 
                        v-model="profile.name"
                        placeholder="Enter company name"
                        required
                      >
                    </div>
                    
                    <div class="form-group mb-3">
                      <label for="contactPerson">Contact Person</label>
                      <input 
                        type="text" 
                        class="form-control" 
                        id="contactPerson" 
                        v-model="profile.contactPerson"
                        placeholder="Enter contact person name"
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
                        placeholder="Enter email address"
                        required
                      >
                    </div>
                  </div>
                  
                  <div class="col-md-6">
                    <div class="form-group mb-3">
                      <label for="phone">Phone</label>
                      <input 
                        type="tel" 
                        class="form-control" 
                        id="phone" 
                        v-model="profile.phone"
                        placeholder="Enter phone number"
                        required
                      >
                    </div>
                    
                    <div class="form-group mb-3">
                      <label for="address">Address</label>
                      <textarea 
                        class="form-control" 
                        id="address" 
                        v-model="profile.address"
                        placeholder="Enter address"
                        rows="2"
                        required
                      ></textarea>
                    </div>
                  </div>
                </div>
                
                <div class="d-flex justify-content-end">
                  <button type="submit" class="btn btn-primary">Save Profile</button>
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
  name: 'ManufacturerPage',
  components: { HeaderManu },
  data() {
    return {
      profile: {
        name: 'Nike',
        contactPerson: 'John Doe',
        email: 'john.doe@nike.com',
        phone: '123-456-7890',
        address: '123 Nike Street, Portland, OR'
      }
    }
  },
  methods: {
    saveProfile() {
      // TODO: Implement API call to save profile
      alert('Profile saved successfully!');
    },
    logout() {
      this.$router.push('/login');
    }
  }
}
</script>

<style>
.manufacturer-layout {
  min-height: 100vh;
  background-color: #f8f9fa;
}

.main-content {
  padding: 20px;
}

.card {
  box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
  border: none;
  margin-bottom: 1.5rem;
}

.card-header {
  border-bottom: 0;
}
</style>
