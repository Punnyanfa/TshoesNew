<template>
  <div class="modal-overlay" v-if="show" @click.self="closeModal">
    <div class="modal-content">
      <div class="modal-header">
        <h2>Add New Template</h2>
        <button class="close-button" @click="closeModal">&times;</button>
      </div>
      <div class="modal-body">
        <form @submit.prevent="handleSubmit">
          <div class="form-group">
            <label>Name:</label>
            <input type="text" v-model="formData.name" required>
          </div>

          <div class="form-group">
            <label>Description:</label>
            <textarea v-model="formData.description" required></textarea>
          </div>

          <div class="form-group">
            <label>Gender:</label>
            <select v-model="formData.gender" required>
              <option value="Male">Male</option>
              <option value="Female">Female</option>
              <option value="Unisex">Unisex</option>
            </select>
          </div>

          <div class="form-group">
            <label>Color:</label>
            <input type="text" v-model="formData.color" required>
          </div>

          <div class="form-group">
            <label>Preview Image:</label>
            <input type="file" @change="handlePreviewImage" accept="image/*" required>
          </div>

          <div class="form-group">
            <label>3D Model File:</label>
            <input type="file" @change="handle3DModel" required>
          </div>

          <div class="form-group">
            <label>Base Price:</label>
            <input type="number" v-model="formData.basePrice" required min="0">
          </div>

          <div class="form-group">
            <label>
              <input type="checkbox" v-model="formData.isAvailable">
              Available
            </label>
          </div>

          <div class="form-actions">
            <button type="button" class="cancel-btn" @click="closeModal">Cancel</button>
            <button type="submit" class="submit-btn">Add Template</button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import { addTemplate } from '@/server/custom-service'

export default {
  name: 'TemplateModal',
  props: {
    show: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      formData: {
        name: '',
        description: '',
        gender: 'Male',
        color: '',
        previewImage: null,
        model3DFile: null,
        basePrice: 0,
        isAvailable: true
      }
    }
  },
  methods: {
    closeModal() {
      this.$emit('close')
      this.resetForm()
    },
    resetForm() {
      this.formData = {
        name: '',
        description: '',
        gender: 'Male',
        color: '',
        previewImage: null,
        model3DFile: null,
        basePrice: 0,
        isAvailable: true
      }
    },
    async handlePreviewImage(event) {
      const file = event.target.files[0]
      if (file) {
        console.log('Preview image:', file.name, file.type, file.size)
        this.formData.previewImage = file
      }
    },
    async handle3DModel(event) {
      const file = event.target.files[0]
      if (file) {
        console.log('3D model:', file.name, file.type, file.size)
        this.formData.model3DFile = file
      }
    },
    async handleSubmit() {
      try {
        if (!this.formData.previewImage || !this.formData.model3DFile) {
          alert('Please select both preview image and 3D model files')
          return
        }

        // Validate all required fields
        if (!this.formData.name || !this.formData.description || !this.formData.color) {
          alert('Please fill in all required fields')
          return
        }

        const formData = new FormData()
        formData.append('Name', this.formData.name.trim())
        formData.append('Description', this.formData.description.trim())
        formData.append('Gender', this.formData.gender)
        formData.append('Color', this.formData.color.trim())
        formData.append('PreviewImage', this.formData.previewImage)
        formData.append('Model3DFile', this.formData.model3DFile)
        formData.append('BasePrice', this.formData.basePrice.toString())
        formData.append('IsAvailable', this.formData.isAvailable.toString())

        console.log('Submitting form data:', {
          Name: this.formData.name,
          Description: this.formData.description,
          Gender: this.formData.gender,
          Color: this.formData.color,
          PreviewImage: this.formData.previewImage.name,
          Model3DFile: this.formData.model3DFile.name,
          BasePrice: this.formData.basePrice,
          IsAvailable: this.formData.isAvailable
        })

        const response = await addTemplate(formData)
        console.log('Response:', response)
        
        // Check if response indicates success
        if (response.code === 200 || (response.message && response.message.toLowerCase().includes('successfully'))) {
          // Emit both events to ensure proper handling
          this.$emit('template-added')
          this.$emit('close')
          this.resetForm()
          
          // Show success message
          alert('Template added successfully!')
        } else {
          throw new Error(response.message || 'Failed to add template')
        }
      } catch (error) {
        console.error('Error creating template:', error)
        alert(`Failed to create template: ${error.message || 'Please try again.'}`)
      }
    }
  }
}
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 1000;
}

.modal-content {
  background-color: white;
  border-radius: 8px;
  width: 500px;
  max-width: 90%;
  max-height: 90vh;
  overflow-y: auto;
}

.modal-header {
  padding: 20px;
  border-bottom: 1px solid #eee;
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.close-button {
  background: none;
  border: none;
  font-size: 24px;
  cursor: pointer;
  color: #666;
}

.modal-body {
  padding: 20px;
}

.form-group {
  margin-bottom: 15px;
}

.form-group label {
  display: block;
  margin-bottom: 5px;
  font-weight: 500;
}

.form-group input[type="text"],
.form-group input[type="number"],
.form-group select,
.form-group textarea {
  width: 100%;
  padding: 8px;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.form-group textarea {
  height: 100px;
  resize: vertical;
}

.form-actions {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  margin-top: 20px;
}

.submit-btn, .cancel-btn {
  padding: 8px 16px;
  border-radius: 4px;
  cursor: pointer;
  border: none;
}

.submit-btn {
  background-color: #4CAF50;
  color: white;
}

.cancel-btn {
  background-color: #f44336;
  color: white;
}

.submit-btn:hover {
  background-color: #45a049;
}

.cancel-btn:hover {
  background-color: #da190b;
}
</style> 