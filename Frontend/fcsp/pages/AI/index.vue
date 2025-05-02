<template>
  <div class="ai-container">
    <div class="card">
      <h2 class="card-title">AI Image Generation</h2>
      
      <form @submit.prevent="handleSubmit" class="ai-form">
        <div class="form-group">
          <label>Prompt</label>
          <input
            v-model="prompt"
            type="text"
            class="input-text"
            placeholder="Enter your prompt here..."
          />
        </div>

        <button type="submit" class="submit-button" :disabled="isLoading">
          {{ isLoading ? 'Generating...' : 'Generate Image' }}
        </button>
      </form>

      <!-- Result Section -->
      <div v-if="generatedImage" class="result-section">
        <h3>Generated Image</h3>
        <div class="image-container">
          <img :src="generatedImage" alt="Generated AI Image" class="generated-image"/>
          <div class="image-actions">
            <a :href="generatedImage" download class="download-button">Download Image</a>
          </div>
        </div>
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
      prompt: '',
      generatedImage: null,
      isLoading: false,
      error: null
    }
  },
  methods: {
    async handleSubmit() {
      this.error = null
      this.isLoading = true
      this.generatedImage = null  // Reset the image before generating new one

      try {
        const formData = new FormData()
        formData.append('Prompt', this.prompt)
        formData.append('OwnerId', localStorage.getItem('userId'))
        formData.append('Status', '0')

        const result = await aiService.generateImage(formData)
        console.log("img", result)
        if (result.data && result.data.imageUrl) {
          this.generatedImage = result.data.imageUrl
        } else {
          throw new Error('No image URL in response')
        }
      } catch (error) {
        this.error = 'Failed to generate image. Please try again.'
        console.error('Error:', error)
      } finally {
        this.isLoading = false
      }
    },
    clearForm() {
      this.prompt = ''
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

.image-container {
  margin: 1rem auto;
  max-width: 100%;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.generated-image {
  width: 100%;
  height: auto;
  display: block;
}

.image-actions {
  padding: 1rem;
  background: #f5f5f5;
  display: flex;
  justify-content: center;
}

.download-button {
  background-color: #4CAF50;
  color: white;
  padding: 0.5rem 1rem;
  border-radius: 4px;
  text-decoration: none;
  font-weight: 500;
  transition: background-color 0.3s;
}

.download-button:hover {
  background-color: #45a049;
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
