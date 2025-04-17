<template>
  <Header />
  <div class="container-fluid">
  <div class="container py-4">
    <div class="row">
      <div class="col-12">
        <h2 class="mb-0">My Profile</h2>
        <p class="text-muted">Manage your profile information to secure your account</p>
        <hr class="my-3" />
      </div>
    </div>

    <div class="row">
      <div class="col-md-8">
        <form @submit.prevent="saveProfile">
          <div class="mb-3 row">
            <label class="col-sm-3 col-form-label text-md-end">Username</label>
            <div class="col-sm-9">
              <span class="form-control-plaintext text-primary">biducbi</span>
            </div>
          </div>

          <div class="mb-3 row">
            <label for="fullName" class="col-sm-3 col-form-label text-md-end">Full Name</label>
            <div class="col-sm-9">
              <input
                type="text"
                class="form-control"
                id="fullName"
                v-model="profile.fullName"
                placeholder="Enter your full name"
              />
            </div>
          </div>

          <div class="mb-3 row">
            <label class="col-sm-3 col-form-label text-md-end">Email</label>
            <div class="col-sm-9 d-flex align-items-center">
              <span class="me-2">ho************@icloud.com</span>
              <a href="#" class="text-primary">Change</a>
            </div>
          </div>

          <div class="mb-3 row">
            <label class="col-sm-3 col-form-label text-md-end">Phone Number</label>
            <div class="col-sm-9 d-flex align-items-center">
              <span class="me-2">*********74</span>
              <a href="#" class="text-primary">Change</a>
            </div>
          </div>

          <div class="mb-3 row">
            <label class="col-sm-3 col-form-label text-md-end">Gender</label>
            <div class="col-sm-9">
              <div class="form-check form-check-inline">
                <input
                  class="form-check-input"
                  type="radio"
                  name="gender"
                  id="male"
                  value="Male"
                  v-model="profile.gender"
                />
                <label class="form-check-label" for="male">Male</label>
              </div>
              <div class="form-check form-check-inline">
                <input
                  class="form-check-input"
                  type="radio"
                  name="gender"
                  id="female"
                  value="Female"
                  v-model="profile.gender"
                />
                <label class="form-check-label" for="female">Female</label>
              </div>
              <div class="form-check form-check-inline">
                <input
                  class="form-check-input"
                  type="radio"
                  name="gender"
                  id="other"
                  value="Other"
                  v-model="profile.gender"
                />
                <label class="form-check-label" for="other">Other</label>
              </div>
            </div>
          </div>

          <div class="mb-3 row">
            <label class="col-sm-3 col-form-label text-md-end">Date of Birth</label>
            <div class="col-sm-9 d-flex align-items-center">
              <span class="me-2">**/*/2002</span>
              <a href="#" class="text-primary">Change</a>
            </div>
          </div>

          <div class="mb-3 row">
            <div class="col-sm-9 offset-sm-3">
              <button type="submit" class="button-btn">Save</button>
            </div>
          </div>
        </form>
      </div>

      <div class="col-md-4 text-center">
        <div class="d-flex flex-column align-items-center">
          <div class="position-relative mb-3">
            <img
              :src="profile.avatar || 'https://hebbkx1anhila5yf.public.blob.vercel-storage.com/image-Pg8Xar0cs5YJAv7GozxRFA8TXVJ7tP.png'"
              alt="Profile Picture"
              class="rounded-circle img-thumbnail"
              style="width: 150px; height: 150px; object-fit: cover"
            />
          </div>
          <button class="btn btn-outline-secondary mb-2" @click="selectImage">Choose Image</button>
          <input
            type="file"
            ref="fileInput"
            @change="onFileChange"
            accept=".jpeg,.jpg,.png"
            style="display: none"
          />
          <p class="text-muted small mb-1">Maximum file size 1 MB</p>
          <p class="text-muted small">Format: .JPEG, .PNG</p>
        </div>
      </div>
    </div>
  </div>
  </div>
  <Footer />
</template>

<script>
export default {
  name: 'ProfilePage',
  data() {
    return {
      profile: {
        username: 'biducbi',
        fullName: 'Hoang Dinh Duc',
        email: 'ho************@icloud.com',
        phone: '*********74',
        gender: 'Male',
        birthdate: '**/*/2002',
        avatar: null
      }
    }
  },
  methods: {
    saveProfile() {
      // Here you would typically send the profile data to your backend
      alert('Profile information has been saved!')
    },
    selectImage() {
      this.$refs.fileInput.click()
    },
    onFileChange(event) {
      const file = event.target.files[0]
      if (!file) return
      
      // Check file size (1MB = 1048576 bytes)
      if (file.size > 1048576) {
        alert('File size is too large. Please choose a file smaller than 1MB.')
        return
      }
      
      // Check file type
      if (!['image/jpeg', 'image/png'].includes(file.type)) {
        alert('Only JPEG or PNG files are accepted.')
        return
      }
      
      // Create a preview
      const reader = new FileReader()
      reader.onload = (e) => {
        this.profile.avatar = e.target.result
      }
      reader.readAsDataURL(file)
    }
  }
}
</script>

<style scoped>
.container-fluid {
  background-color: #f8f9fa;
  padding-top: 100px;
  padding-bottom: 100px;
}
.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem;
  background: #fff;
  border-radius: 12px;
  box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
  margin-top: 2rem;
  margin-bottom: 2rem;
}

h2 {
  color: #2c3e50;
  font-weight: 700;
  font-size: 2rem;
  margin-bottom: 0.5rem;
}

.text-muted {
  color: #6c757d;
  font-size: 0.9rem;
}

hr {
  border-color: #e9ecef;
  margin: 1.5rem 0;
}

.form-control {
  border: 1px solid #dee2e6;
  border-radius: 8px;
  padding: 0.75rem 1rem;
  transition: all 0.3s ease;
}

.form-control:focus {
  border-color: #3498db;
  box-shadow: 0 0 0 0.2rem rgba(52, 152, 219, 0.25);
}

.form-control-plaintext {
  color: #2c3e50;
  font-weight: 500;
}

.text-primary {
  color: #3498db !important;
  text-decoration: none;
  transition: color 0.3s ease;
}

.text-primary:hover {
  color: #2980b9 !important;
}

.form-check-input {
  cursor: pointer;
}

.form-check-input:checked {
  background-color: #3498db;
  border-color: #3498db;
}

.btn-danger {
  background: linear-gradient(45deg, #e74c3c, #c0392b);
  border: none;
  padding: 0.75rem 2rem;
  font-weight: 600;
  border-radius: 8px;
  transition: all 0.3s ease;
}

.btn-danger:hover {
  background: linear-gradient(45deg, #c0392b, #e74c3c);
  transform: translateY(-2px);
  box-shadow: 0 4px 15px rgba(231, 76, 60, 0.3);
}

.btn-outline-secondary {
  border: 2px solid #3498db;
  color: #3498db;
  padding: 0.5rem 1.5rem;
  border-radius: 8px;
  font-weight: 500;
  transition: all 0.3s ease;
}

.btn-outline-secondary:hover {
  background: #3498db;
  color: white;
  transform: translateY(-2px);
}

.img-thumbnail {
  border: 3px solid #3498db;
  padding: 0.25rem;
  transition: all 0.3s ease;
}

.img-thumbnail:hover {
  transform: scale(1.05);
  box-shadow: 0 4px 15px rgba(52, 152, 219, 0.3);
}

.small {
  font-size: 0.8rem;
  color: #6c757d;
}

/* Responsive styles */
@media (max-width: 768px) {
  .container {
    padding: 1rem;
    margin: 1rem;
  }

  h2 {
    font-size: 1.5rem;
  }

  .col-form-label {
    text-align: left !important;
    margin-bottom: 0.5rem;
  }

  .btn-danger {
    width: 100%;
  }
}

/* Animation for form elements */
.form-control, .btn, .form-check-input {
  transition: all 0.3s ease;
}

/* Custom radio button styles */
.form-check-input {
  width: 1.2em;
  height: 1.2em;
  margin-top: 0.2em;
}

.form-check-label {
  margin-left: 0.5rem;
  cursor: pointer;
}

/* Profile picture container */
.position-relative {
  position: relative;
  overflow: hidden;
  border-radius: 50%;
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
}

/* File input styling */
input[type="file"] {
  display: none;
}

.button-btn {
  background: linear-gradient(45deg, #2c3e50, #3498db);
  color: #fff;
  border: none;
  border-radius: 50px;
  padding: 12px 30px;
  font-weight: 600;
  font-size: 1rem;
  letter-spacing: 0.5px;
  text-transform: uppercase;
  transition: all 0.3s ease;
  box-shadow: 0 4px 15px rgba(52, 152, 219, 0.2);
  position: relative;
  overflow: hidden;
  cursor: pointer;
  width: 100%;
  max-width: 200px;
}

.button-btn:hover {
  background: linear-gradient(45deg, #3498db, #2c3e50);
  transform: translateY(-2px);
  box-shadow: 0 6px 20px rgba(52, 152, 219, 0.3);
}

.button-btn:active {
  transform: translateY(0);
  box-shadow: 0 2px 10px rgba(52, 152, 219, 0.2);
}

.button-btn::before {
  content: '';
  position: absolute;
  top: 0;
  left: -100%;
  width: 100%;
  height: 100%;
  background: linear-gradient(
    90deg,
    transparent,
    rgba(255, 255, 255, 0.2),
    transparent
  );
  transition: 0.5s;
}

.button-btn:hover::before {
  left: 100%;
}

@media (max-width: 768px) {
  .button-btn {
    max-width: 100%;
    padding: 10px 20px;
    font-size: 0.9rem;
  }
}

/* Profile section spacing */
.mb-3 {
  margin-bottom: 1.5rem !important;
}

/* Form row spacing */
.row {
  margin-bottom: 1rem;
}

/* Custom scrollbar */
::-webkit-scrollbar {
  width: 8px;
}

::-webkit-scrollbar-track {
  background: #f1f1f1;
}

::-webkit-scrollbar-thumb {
  background: #3498db;
  border-radius: 4px;
}

::-webkit-scrollbar-thumb:hover {
  background: #2980b9;
}
</style>