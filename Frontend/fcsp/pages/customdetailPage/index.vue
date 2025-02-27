<template>
  <div class="page-wrapper">
    <!-- Header Component -->
    <Header />

    <!-- Main Content Section -->
    <main class="main-content">
      <div class="customizer-grid">
        <!-- Main Product Area (Left) -->
        <div class="product-area">
          <div class="three-canvas">
            <!-- Khối 2D ban đầu -->
            <div v-if="!show3D" class="preview-block"></div>
            <!-- Canvas 3D chỉ hiển thị khi show3D = true -->
            <div v-show="show3D" ref="container" class="three-canvas-inner">
              <div v-if="!isModelLoaded" class="loading-overlay">
                <span class="loading-text">Loading 3D Model...</span>
              </div>
            </div>
            <!-- Nút dấu cộng -->
            <button class="expand-btn" @click="toggle3DAnimation" :disabled="isAnimating || !isModelLoaded">
              <span>+</span>
            </button>
          </div>
        </div>

        <!-- Product Details and Controls (Right) -->
        <div class="details-section">
          <h1 class="product-title">Customizable Lightweight Breathable Running Sneakers</h1>
          <button class="customize-btn" @click="handleCustomize" :disabled="!isModelLoaded || !show3D">
            <span v-if="!isModelLoaded || !show3D">Loading...</span>
            <span v-else>Customize Now</span>
          </button>

          <!-- Product Details -->
          <div class="product-details">
            <p class="price">2,587,000₫</p>
            <div class="detail-row">
              <span>Color:</span>
              <span class="detail-value">White</span>
            </div>
            <div class="detail-row">
              <span>Size:</span>
              <select class="size-select">
                <option>Women US 5.5 (EU 36)</option>
                <option>Women US 6 (EU 37)</option>
                <option>Women US 6.5 (EU 38)</option>
                <option>Women US 7 (EU 39)</option>
              </select>
            </div>
            <div class="detail-row description-toggle" @click="toggleDescription">
              <span>Description</span>
              <button class="toggle-btn">{{ showDescription ? '▲' : '▼' }}</button>
            </div>
            <div v-if="showDescription" class="description-content">
              <p>Experience ultimate comfort and style with these lightweight, breathable running sneakers. Perfect for daily runs, workouts, or casual wear, featuring a durable sole and premium fabric for all-day support.</p>
            </div>
          </div>
        </div>
      </div>
    </main>

    <!-- Footer Component -->
    <Footer />
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue';
import * as THREE from 'three';
import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls';

// Refs cho Three.js container và states
const container = ref(null);
const isModelLoaded = ref(false);
const showDescription = ref(false);
const isAnimating = ref(false);
const show3D = ref(false); // Trạng thái hiển thị 3D
let mixer = null;

onMounted(() => {
  const scene = new THREE.Scene();
  const camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);
  camera.position.set(3, 3, 6);

  const renderer = new THREE.WebGLRenderer({ antialias: true });
  renderer.setSize(window.innerWidth * 0.5, window.innerHeight * 0.8);

  const light = new THREE.AmbientLight(0xffffff, 0.9);
  scene.add(light);

  const directionalLight = new THREE.DirectionalLight(0xffffff, 1.2);
  directionalLight.position.set(5, 10, 5);
  scene.add(directionalLight);

  const controls = new OrbitControls(camera, renderer.domElement);
  controls.enableDamping = true;
  controls.minDistance = 2;
  controls.maxDistance = 10;

  const loader = new GLTFLoader();
  loader.load(
    '/Nike jordan4.glb',
    (gltf) => {
      console.log('✅ Model loaded successfully:', gltf);
      scene.add(gltf.scene);

      // Khởi tạo AnimationMixer nếu model có animation
      if (gltf.animations && gltf.animations.length > 0) {
        mixer = new THREE.AnimationMixer(gltf.scene);
        const action = mixer.clipAction(gltf.animations[0]);
        action.setLoop(THREE.LoopOnce);
        action.clampWhenFinished = true;
      }

      isModelLoaded.value = true;
    },
    (xhr) => {
      console.log(`Loading progress: ${(xhr.loaded / xhr.total) * 100}%`);
    },
    (error) => {
      console.error('❌ Error loading model:', error);
      isModelLoaded.value = false;
    }
  );

  const clock = new THREE.Clock();
  const animate = () => {
    requestAnimationFrame(animate);
    controls.update();

    if (mixer && isAnimating.value) {
      mixer.update(clock.getDelta());
    }

    renderer.render(scene, camera);
  };
  animate();

  // Gắn renderer khi chuyển sang 3D
  watch(show3D, (newVal) => {
    if (newVal && container.value) {
      container.value.appendChild(renderer.domElement);
    }
  });

  window.addEventListener('resize', () => {
    camera.aspect = window.innerWidth * 0.5 / (window.innerHeight * 0.8);
    camera.updateProjectionMatrix();
    renderer.setSize(window.innerWidth * 0.5, window.innerHeight * 0.8);
  });
});

// Chuyển sang 3D và chạy animation
const toggle3DAnimation = () => {
  if (isModelLoaded.value) {
    show3D.value = true;
    isAnimating.value = true;
    if (mixer) {
      const action = mixer.getRoot().animations[0] ? mixer.clipAction(mixer.getRoot().animations[0]) : null;
      if (action) {
        action.reset().play();
        action.getClip().duration && setTimeout(() => {
          isAnimating.value = false;
        }, action.getClip().duration * 1000);
      }
    } else {
      // Animation mặc định nếu không có animation trong GLB
      const model = container.value.querySelector('canvas')?.parentElement.__vue__.$parent.scene.children.find(child => child.isGroup);
      if (model) {
        const animateModel = () => {
          if (isAnimating.value) {
            model.rotation.y += 0.02;
            requestAnimationFrame(animateModel);
          }
        };
        animateModel();
        setTimeout(() => isAnimating.value = false, 3000); // Dừng sau 3 giây
      }
    }
  }
};

// Handle customize button click
const handleCustomize = () => {
  if (isModelLoaded.value && show3D.value) {
    alert('Customize functionality will open here! (Implement your customization logic.)');
  }
};

// Toggle description visibility
const toggleDescription = () => {
  showDescription.value = !showDescription.value;
};
</script>

<style scoped>
/* General Layout */
.page-wrapper {
  min-height: 100vh;
  background: linear-gradient(135deg, #f5f7fa 0%, #e0e8f5 100%);
}

/* Main Content */
.main-content {
  padding: 2rem;
  max-width: 1200px;
  margin: 0 auto;
}

/* Customizer Grid */
.customizer-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 2.5rem;
  align-items: start;
}

/* Product Area (Left) */
.product-area {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.three-canvas {
  width: 100%;
  max-width: 500px;
  height: 400px;
  background-color: #ffffff;
  border-radius: 12px;
  box-shadow: 0 5px 20px rgba(0, 0, 0, 0.1);
  overflow: hidden;
  position: relative;
  transition: transform 0.3s ease, box-shadow 0.3s ease;
}

.three-canvas:hover {
  transform: scale(1.02);
  box-shadow: 0 8px 25px rgba(0, 0, 0, 0.15);
}

.three-canvas-inner {
  width: 100%;
  height: 100%;
}

/* Preview Block */
.preview-block {
  width: 100%;
  height: 100%;
  background-image: url('data:image/jpeg;base64,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');
  background-size: cover;
  background-position: center;
  border-radius: 12px;
  box-shadow: inset 0 4px 8px rgba(0, 0, 0, 0.1);
  display: flex;
  align-items: center;
  justify-content: center;
}

.preview-block::before {
  content: "Preview";
  font-size: 1.5rem;
  color: #666;
  font-weight: 600;
}

/* Loading Overlay */
.loading-overlay {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: rgba(255, 255, 255, 0.9);
  display: flex;
  justify-content: center;
  align-items: center;
  border-radius: 12px;
}

.loading-text {
  font-size: 1.2rem;
  color: #1a3c6c;
  font-weight: 600;
  animation: pulse 1.5s infinite;
}

@keyframes pulse {
  0% { opacity: 1; }
  50% { opacity: 0.7; }
  100% { opacity: 1; }
}

/* Nút dấu cộng */
.expand-btn {
  position: absolute;
  bottom: 20px;
  right: 20px;
  width: 40px;
  height: 40px;
  background-color: #8bc34a;
  border: none;
  border-radius: 50%;
  color: #ffffff;
  font-size: 1.5rem;
  font-weight: 700;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: background-color 0.3s ease, transform 0.3s ease, box-shadow 0.3s ease;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.2);
}

.expand-btn:hover:not(:disabled) {
  background-color: #8bc34a;
  transform: scale(1.1);
  box-shadow: 0 6px 15px rgba(0, 0, 0, 0.25);
}

.expand-btn:disabled {
  background-color: #a0d9b4;
  cursor: not-allowed;
  opacity: 0.7;
}

.expand-btn:active:not(:disabled) {
  transform: scale(0.95);
}

/* Details Section (Right) */
.details-section {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
  background: #ffffff;
  padding: 2rem;
  border-radius: 12px;
  box-shadow: 0 5px 20px rgba(0, 0, 0, 0.1);
  transition: transform 0.3s ease;
}

.details-section:hover {
  transform: translateY(-5px);
}

.product-title {
  font-size: 1.8rem;
  font-weight: 700;
  color: #1a3c6c;
  margin: 0;
  text-transform: uppercase;
  letter-spacing: 1px;
  text-shadow: 1px 1px 2px rgba(0, 0, 0, 0.05);
}

.customize-btn {
  padding: 1rem 2.5rem;
  font-size: 1.1rem;
  font-weight: 700;
  color: #ffffff;
  background-color: #8bc34a;
  border: none;
  border-radius: 25px;
  cursor: pointer;
  transition: background-color 0.3s ease, transform 0.2s ease, box-shadow 0.3s ease;
  box-shadow: 0 5px 15px rgba(40, 167, 69, 0.3);
}

.customize-btn:hover:not(:disabled) {
  background-color: #8bc34a;
  transform: translateY(-3px);
  box-shadow: 0 8px 20px rgba(40, 167, 69, 0.4);
}

.customize-btn:active:not(:disabled) {
  transform: translateY(0);
  box-shadow: 0 3px 10px rgba(40, 167, 69, 0.3);
}

.customize-btn:disabled {
  background-color: #a0d9b4;
  cursor: not-allowed;
  opacity: 0.7;
}

/* Product Details */
.product-details {
  margin-top: 1.5rem;
}

.price {
  font-size: 1.4rem;
  font-weight: 700;
  color: #1a3c6c;
  margin-bottom: 1rem;
  text-align: right;
}

.detail-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
  font-size: 1rem;
  color: #444;
  transition: color 0.3s ease;
}

.detail-row:hover {
  color: #1a3c6c;
}

.detail-value {
  font-weight: 500;
  color: #333;
}

.size-select {
  padding: 0.5rem;
  border: 2px solid #ccc;
  border-radius: 6px;
  font-size: 1rem;
  background-color: #fff;
  transition: border-color 0.3s ease, box-shadow 0.3s ease;
}

.size-select:focus {
  outline: none;
  border-color: #28a745;
  box-shadow: 0 0 5px rgba(40, 167, 69, 0.3);
}

.description-toggle {
  cursor: pointer;
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.8rem 0;
  border-top: 2px solid #eee;
  transition: color 0.3s ease, transform 0.3s ease;
}

.description-toggle:hover {
  color: #1a3c6c;
  transform: translateX(5px);
}

.toggle-btn {
  background: none;
  border: none;
  font-size: 1rem;
  color: #666;
  cursor: pointer;
  transition: color 0.3s ease;
}

.toggle-btn:hover {
  color: #1a3c6c;
}

.description-content {
  margin-top: 1rem;
  padding: 1rem;
  background: #f9f9f9;
  border-radius: 8px;
  box-shadow: inset 0 2px 5px rgba(0, 0, 0, 0.05);
  font-size: 0.9rem;
  color: #555;
  line-height: 1.6;
  animation: fadeIn 0.5s ease-in;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(-10px); }
  to { opacity: 1; transform: translateY(0); }
}

/* Responsive Design */
@media (max-width: 768px) {
  .customizer-grid {
    grid-template-columns: 1fr;
    gap: 1.5rem;
  }

  .three-canvas {
    max-width: 100%;
    height: 300px;
  }

  .details-section {
    padding: 1.5rem;
  }

  .product-title {
    font-size: 1.4rem;
  }

  .customize-btn {
    padding: 0.8rem 2rem;
    font-size: 1rem;
  }

  .price {
    font-size: 1.2rem;
  }

  .detail-row {
    font-size: 0.9rem;
  }
}
</style>
