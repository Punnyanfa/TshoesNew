<!-- SidebarCustom.vue -->
<template>
  <div class="sidebar-custom">
    <!-- Tabs for different customization options -->
    <div class="tabs">
      <button 
        class="tab-btn" 
        :class="{ active: activeTab === 'text' }" 
        @click="activeTab = 'text'"
      >
        <i class="icon-text">T</i> Text
      </button>
      <button 
        class="tab-btn" 
        :class="{ active: activeTab === 'background' }" 
        @click="activeTab = 'background'"
      >
        <i class="icon-background">□</i> Background
      </button>
      <button 
        class="tab-btn" 
        :class="{ active: activeTab === 'upload' }" 
        @click="activeTab = 'upload'"
      >
        <i class="icon-upload">☁</i> My Files
      </button>
    </div>

    <!-- Tab Content -->
    <div class="tab-content">
      <!-- Upload Image Tab -->
      <div v-if="activeTab === 'upload'" class="upload-section">
        <label for="image-upload" class="upload-btn">
          Upload
        </label>
        <input 
          id="image-upload" 
          type="file" 
          accept="image/*" 
          @change="handleImageUpload" 
          style="display: none;"
        />
        <p v-if="uploadedImage" class="uploaded-file-name">
          Uploaded: {{ uploadedImage.name }}
        </p>
        <button 
          v-if="uploadedImage" 
          class="apply-btn" 
          @click="applyTextureToModel"
          :disabled="isApplying"
        >
          {{ isApplying ? 'Applying...' : 'Apply Texture' }}
        </button>
      </div>

      <!-- Text Tab -->
      <div v-if="activeTab === 'text'" class="text-section">
        <input 
          v-model="customText" 
          type="text" 
          placeholder="Enter your text here" 
          class="text-input"
          @input="applyText"
        />
      </div>

      <!-- Background Tab -->
      <div v-if="activeTab === 'background'" class="background-section">
        <div class="color-options">
          <div 
            v-for="color in backgroundColors" 
            :key="color" 
            class="color-swatch" 
            :style="{ backgroundColor: color }"
            @click="applyBackgroundColor(color)"
          ></div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, watch } from 'vue';
import * as THREE from 'three';

const props = defineProps({
  scene: {
    type: Object,
    required: true,
  },
  model: {
    type: Object,
    default: null,
  },
});

const emit = defineEmits(['textureApplied', 'textApplied', 'backgroundColorApplied']);

const activeTab = ref('upload');
const uploadedImage = ref(null);
const isApplying = ref(false);
const customText = ref(''); // Text input value
const backgroundColors = ref([
  '#ffffff', '#ff6b6b', '#4ecdc4', '#45b7d1', 
  '#96c93d', '#ffe66d', '#6b7280', '#000000'
]);

// Handle image upload
const handleImageUpload = (event) => {
  const file = event.target.files[0];
  if (file && file.type.startsWith('image/')) {
    uploadedImage.value = file;
  } else {
    alert('Please upload a valid image file.');
  }
};

// Apply uploaded image as texture
const applyTextureToModel = () => {
  if (!uploadedImage.value || !props.model) return;

  isApplying.value = true;
  const reader = new FileReader();
  reader.onload = (e) => {
    const textureLoader = new THREE.TextureLoader();
    textureLoader.load(
      e.target.result,
      (texture) => {
        props.model.traverse((child) => {
          if (child.isMesh) {
            const newMaterial = new THREE.MeshStandardMaterial({
              map: texture,
              metalness: 0.2,
              roughness: 0.8,
            });
            child.material = newMaterial;
            child.material.needsUpdate = true;
          }
        });
        isApplying.value = false;
        emit('textureApplied', texture);
        alert('Texture applied successfully!');
      },
      undefined,
      (error) => {
        console.error('Error loading texture:', error);
        isApplying.value = false;
        alert('Failed to apply texture. Please try again.');
      }
    );
  };
  reader.readAsDataURL(uploadedImage.value);
};

// Apply text (you might want to implement this based on your needs)
const applyText = () => {
  if (customText.value && props.scene) {
    emit('textApplied', customText.value);
    // Add your text implementation here (e.g., adding text to scene)
  }
};

// Apply background color
const applyBackgroundColor = (color) => {
  if (props.scene) {
    props.scene.background = new THREE.Color(color);
    emit('backgroundColorApplied', color);
  }
};

watch(() => props.model, (newModel) => {
  if (newModel) {
    console.log('Model ready for texture application:', newModel);
  }
});
</script>

<style scoped>
.sidebar-custom {
  width: 250px;
  height: 600px;
  background: #2a3b5a;
  color: #ffffff;
  padding: 1.5rem;
  margin: 50px 0 0 0;
  border-radius: 12px;
  box-shadow: 0 5px 20px rgba(0, 0, 0, 0.2);
  position: fixed;
  top: 50%;
  left: 2rem;
  transform: translateY(-50%);
  z-index: 10;
  transition: all 0.3s ease;
}

/* Tabs */
.tabs {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  margin-bottom: 1.5rem;
}

.tab-btn {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.8rem 1rem;
  background: #1a263d;
  border: none;
  border-radius: 8px;
  color: #b0b8cc;
  font-size: 1rem;
  cursor: pointer;
  transition: all 0.3s ease;
}

.tab-btn:hover {
  background: #222f4e;
  color: #ffffff;
}

.tab-btn.active {
  background: #8bc34a;
  color: #ffffff;
}

/* Tab Content */
.tab-content {
  padding: 1rem 0;
}

.upload-section {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

/* Text Section */
.text-section {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.text-input {
  padding: 0.6rem 1rem;
  border: none;
  border-radius: 8px;
  background: #1a263d;
  color: #ffffff;
  font-size: 0.9rem;
}

.text-input::placeholder {
  color: #b0b8cc;
}

.text-input:focus {
  outline: none;
  box-shadow: 0 0 5px rgba(139, 195, 74, 0.5);
}

/* Background Section */
.background-section {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.color-options {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 0.5rem;
}

.color-swatch {
  width: 100%;
  height: 30px;
  border-radius: 4px;
  cursor: pointer;
  border: 2px solid transparent;
  transition: all 0.2s ease;
}

.color-swatch:hover {
  border: 2px solid #ffffff;
  transform: scale(1.1);
}

/* Existing button styles */
.upload-btn {
  display: inline-block;
  padding: 0.8rem 2rem;
  background: #8bc34a;
  border-radius: 25px;
  text-align: center;
  cursor: pointer;
  font-weight: 600;
  transition: all 0.3s ease;
}

.upload-btn:hover {
  background: #7ab33f;
  transform: translateY(-2px);
  box-shadow: 0 5px 15px rgba(40, 167, 69, 0.3);
}

.apply-btn {
  padding: 0.6rem 1.5rem;
  background: #28a745;
  border: none;
  border-radius: 25px;
  color: #ffffff;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
}

/* Responsive Design */
@media (max-width: 768px) {
  .sidebar-custom {
    width: 200px;
    left: 1rem;
    padding: 1rem;
  }

  .color-options {
    grid-template-columns: repeat(3, 1fr);
  }
}
</style>