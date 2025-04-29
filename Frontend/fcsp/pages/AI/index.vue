<template>
  <div class="ai-container">
    <div class="card">
      <h2 class="card-title">AI Image Generation</h2>
      
      <form @submit.prevent="handleSubmit" class="ai-form">
        <div class="form-group">
          <label>Image File</label>
          <div class="file-input">
            <input
              type="file"
              @change="handleFileChange"
              accept="image/*"
              class="input-file"
            />
            <span class="file-name">{{ fileName || 'No file chosen' }}</span>
          </div>
        </div>

        <div class="form-group">
          <label>Prompt</label>
          <input
            v-model="prompt"
            type="text"
            class="input-text"
            placeholder="Enter your prompt here..."
          />
        </div>

        <div class="form-group">
          <label>Owner ID</label>
          <input
            v-model="ownerId"
            type="number"
            class="input-text"
          />
        </div>

        <div class="form-group">
          <label>Status</label>
          <select v-model="status" class="input-select">
            <option value="">Select status</option>
            <option value="1">Active</option>
            <option value="0">Inactive</option>
          </select>
        </div>

        <button type="submit" class="submit-button" :disabled="isLoading">
          {{ isLoading ? 'Generating...' : 'Generate Image' }}
        </button>
      </form>

      <!-- Result Section -->
      <div v-if="generatedImage" class="result-section">
        <h3>Generated Image</h3>
        <img :src="generatedImage" alt="Generated AI Image" class="generated-image"/>
      </div>

      <!-- Error Message -->
      <div v-if="error" class="error-message">
        {{ error }}
      </div>
    </div>
  </div>
</template>

<script>
import aiService from '@/server/ai-service'

export default {
  name: 'AIImageGenerator',
  data() {
    return {
      imageFile: null,
      fileName: '',
      prompt: '',
      ownerId: '',
      status: '',
      generatedImage: null,
      isLoading: false,
      error: null
    }
  },
  methods: {
    handleFileChange(event) {
      const file = event.target.files[0]
      if (file) {
        this.imageFile = file
        this.fileName = file.name
      }
    },
    async handleSubmit() {
      this.error = null
      this.isLoading = true

      try {
        const formData = new FormData()
        if (this.imageFile) {
          formData.append('ImageFile', this.imageFile)
        }
        formData.append('Prompt', this.prompt)
        formData.append('OwnerId', this.ownerId)
        formData.append('Status', this.status)

        const result = await aiService.generateImage(formData)
        this.generatedImage = result.imageUrl // Adjust based on your API response structure
        
        // Clear form after successful generation
        this.clearForm()
      } catch (error) {
        this.error = 'Failed to generate image. Please try again.'
        console.error('Error:', error)
      } finally {
        this.isLoading = false
      }
    },
    clearForm() {
      this.imageFile = null
      this.fileName = ''
      this.prompt = ''
      this.ownerId = ''
      this.status = ''
    }
  }
}
</script>

<style scoped>
.ai-container {
  max-width: 800px;
  margin: 2rem auto;
  padding: 0 1rem;
}

.card {
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  padding: 2rem;
}

.card-title {
  color: #333;
  margin-bottom: 2rem;
  text-align: center;
}

.ai-form {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.form-group label {
  font-weight: 600;
  color: #555;
}

.input-text, .input-select {
  padding: 0.75rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 1rem;
}

.file-input {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.input-file {
  flex: 1;
}

.file-name {
  color: #666;
}

.submit-button {
  background-color: #4CAF50;
  color: white;
  padding: 1rem;
  border: none;
  border-radius: 4px;
  font-size: 1rem;
  cursor: pointer;
  transition: background-color 0.3s;
}

.submit-button:not(:disabled):hover {
  background-color: #45a049;
}

.submit-button:disabled {
  background-color: #cccccc;
  cursor: not-allowed;
}

.result-section {
  margin-top: 2rem;
  text-align: center;
}

.generated-image {
  max-width: 100%;
  margin-top: 1rem;
  border-radius: 4px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.error-message {
  margin-top: 1rem;
  padding: 1rem;
  background-color: #ffebee;
  color: #c62828;
  border-radius: 4px;
  text-align: center;
}
</style>
