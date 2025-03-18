<template>
  <div class="sidebar-custom">
    <!-- Tabs -->
    <div class="tabs">
      <button class="tab-btn" :class="{ active: activeTab === 'text' }" @click="activeTab = 'text'">
        <i class="icon-text">T</i> Text
      </button>
      <button class="tab-btn" :class="{ active: activeTab === 'background' }" @click="activeTab = 'background'">
        <i class="icon-background">□</i> Background
      </button>
      <button class="tab-btn" :class="{ active: activeTab === 'upload' }" @click="activeTab = 'upload'">
        <i class="icon-upload">☁</i> My Files
      </button>
    </div>

    <!-- Tab Content -->
    <div class="tab-content">
      <!-- Upload Image Tab -->
      <div v-if="activeTab === 'upload'" class="upload-section">
        <label for="image-upload" class="upload-btn">Upload Image</label>
        <input
          id="image-upload"
          type="file"
          accept="image/*"
          @change="handleImageUpload"
          style="display: none;"
        />
        <div v-if="uploadedImage" class="uploaded-file-info">
          <div class="image-preview" 
               draggable="true"
               @dragstart="handleDragStart"
               @dragend="handleDragEnd">
            <img :src="imagePreviewUrl" alt="Uploaded Image Preview" />
          </div>
        </div>
        <p v-if="uploadError" class="error-text">{{ uploadError }}</p>
      </div>

      <!-- Text Tab -->
      <div v-if="activeTab === 'text'" class="text-section">
        <input
          v-model="customText"
          type="text"
          placeholder="Enter your text here"
          class="text-input"
          @input="previewText"
        />
        <button class="apply-btn" @click="applyTextToModel" :disabled="!customText || isApplyingText">
          {{ isApplyingText ? 'Applying...' : 'Apply Text' }}
        </button>
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
        <p class="selected-color">Selected: {{ selectedBackgroundColor }}</p>
      </div>

      <!-- Reset Button -->
      <button class="reset-btn" @click="resetModel">Reset</button>
    </div>
  </div>
</template>

<script setup>
import { ref, watch } from 'vue';
import * as THREE from 'three';

const props = defineProps({
  scene: { type: Object, required: true },
  model: { type: Object, default: null },
  originalMaterials: { type: Map, default: () => new Map() },
});

const emit = defineEmits(['textureApplied', 'textApplied', 'backgroundColorApplied']);

const activeTab = ref('upload');
const uploadedImage = ref(null);
const imagePreviewUrl = ref('');
const isApplying = ref(false);
const uploadError = ref('');
const customText = ref('');
const isApplyingText = ref(false);
const backgroundColors = ref([
  '#ffffff', '#ff6b6b', '#4ecdc4', '#45b7d1',
  '#96c93d', '#ffe66d', '#6b7280', '#000000',
]);
const selectedBackgroundColor = ref('#ffffff');

// Handle image upload
const handleImageUpload = (event) => {
  const file = event.target.files[0];
  uploadError.value = '';
  if (file) {
    if (!file.type.startsWith('image/')) {
      uploadError.value = 'Vui lòng tải lên một file ảnh hợp lệ.';
      return;
    }
    if (file.size > 5 * 1024 * 1024) {
      uploadError.value = 'Kích thước file vượt quá giới hạn 5MB.';
      return;
    }
    uploadedImage.value = file;

    // Tạo preview URL và áp dụng texture ngay lập tức
    const reader = new FileReader();
    reader.onload = (e) => {
      imagePreviewUrl.value = e.target.result;
      // Áp dụng texture ngay sau khi tải ảnh
      emit('textureApplied', e.target.result);
    };
    reader.readAsDataURL(file);
  }
};

// Create text texture using Canvas
const createTextTexture = (text) => {
  const canvas = document.createElement('canvas');
  const context = canvas.getContext('2d');
  canvas.width = 512;
  canvas.height = 256;

  context.clearRect(0, 0, canvas.width, canvas.height);
  context.font = 'Bold 60px Arial';
  context.fillStyle = '#ffffff';
  context.textAlign = 'center';
  context.textBaseline = 'middle';
  context.fillText(text, canvas.width / 2, canvas.height / 2);

  const texture = new THREE.CanvasTexture(canvas);
  texture.needsUpdate = true;
  return texture;
};

// Apply text to purple parts of the model
const applyTextToModel = () => {
  if (!customText.value || !props.model) return;

  isApplyingText.value = true;

  const textTexture = createTextTexture(customText.value);
  props.model.traverse((child) => {
    if (child.isMesh) {
      // Check if the material's color is purple
      const material = child.material;
      let isPurple = false;

      if (material.color) {
        const color = material.color; // THREE.Color object
        const r = color.r * 255;
        const g = color.g * 255;
        const b = color.b * 255;

        // Define a range for purple (adjust as needed)
        if (r > 100 && g < 50 && b > 100) {
          isPurple = true;
        }
      }

      // Apply text texture only to purple parts
      if (isPurple) {
        const newMaterial = new THREE.MeshStandardMaterial({
          map: textTexture,
          transparent: true,
          metalness: 0.2,
          roughness: 0.8,
        });
        child.material.dispose();
        child.material = newMaterial;
        child.material.needsUpdate = true;
      }
    }
  });

  isApplyingText.value = false;
  emit('textApplied', customText.value);
  alert(`Text "${customText.value}" applied to purple parts of the model!`);
};

// Preview text (optional)
const previewText = () => {
  // Add preview logic if needed
};

// Apply background color to the model
const applyBackgroundColor = (color) => {
  if (!props.model) return;

  props.model.traverse((child) => {
    if (child.isMesh) {
      const newMaterial = new THREE.MeshStandardMaterial({
        color: new THREE.Color(color),
        metalness: 0.2,
        roughness: 0.8,
      });
      child.material.dispose();
      child.material = newMaterial;
      child.material.needsUpdate = true;
    }
  });

  selectedBackgroundColor.value = color;
  emit('backgroundColorApplied', color);
  alert(`Color "${color}" applied in the model!`);
};

// Reset model to original state
const resetModel = () => {
  if (!props.model) return;
  props.model.traverse((child) => {
    if (child.isMesh && props.originalMaterials.has(child)) {
      child.material.dispose();
      child.material = props.originalMaterials.get(child).clone();
      child.material.needsUpdate = true;
    }
  });
  uploadedImage.value = null;
  imagePreviewUrl.value = '';
  customText.value = '';
  selectedBackgroundColor.value = '#ffffff';
  document.getElementById('image-upload').value = '';
  alert('Model reset to original state!');
};

const handleDragStart = (event) => {
  event.dataTransfer.setData('text/plain', ''); // Cần thiết cho một số trình duyệt
  emit('textureApplied', imagePreviewUrl.value);
};

const handleDragEnd = () => {
  // Có thể thêm logic xử lý khi kết thúc drag nếu cần
};

watch(() => props.model, (newModel) => {
  if (newModel) {
    console.log('Model ready for customization:', newModel);
  }
});
</script>

<style scoped>
.sidebar-custom {
  width: 300px;
  height: 100vh;
  background: #2a3b5a;
  color: #ffffff;
  padding: 1.5rem;
  border-radius: 12px;
  box-shadow: 0 5px 20px rgba(0, 0, 0, 0.2);
  position: fixed;
  top: 0;
  left: 0;
  transform: none;
  z-index: 10;
  transition: all 0.3s ease;
}

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

.tab-content {
  padding: 1rem 0;
}

.upload-section {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.uploaded-file-info {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.image-preview {
  width: 100%;
  height: 250px;
  margin: 10px 0;
  border-radius: 8px;
  overflow: hidden;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  background: #ffffff;
  cursor: grab;
}

.image-preview img {
  width: 100%;
  height: 100%;
  object-fit: contain;
  pointer-events: none;
}

.image-preview:hover {
  transform: scale(1.02);
  box-shadow: 0 6px 16px rgba(0, 0, 0, 0.2);
  transition: all 0.3s ease;
}

.error-text {
  color: #ff4444;
  font-size: 0.9rem;
  margin: 0;
}

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

.selected-color {
  font-size: 0.9rem;
  margin: 0;
}

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

.apply-btn:hover {
  background: #218838;
}

.apply-btn:disabled {
  background: #6c757d;
  cursor: not-allowed;
}

.reset-btn {
  padding: 0.6rem 1.5rem;
  background: #ff4444;
  border: none;
  border-radius: 25px;
  color: #ffffff;
  font-weight: 600;
  cursor: pointer;
  transition: all 0.3s ease;
  margin-top: 1rem;
}

.reset-btn:hover {
  background: #cc0000;
}

@media (max-width: 768px) {
  .sidebar-custom {
    width: 100%;
    height: auto;
    position: relative;
    left: 0;
    padding: 10px;
  }
  
  .tabs {
    flex-direction: row;
    flex-wrap: wrap;
  }
  
  .tab-btn {
    flex: 1 1 auto;
    min-width: 120px;
  }
}
</style>
