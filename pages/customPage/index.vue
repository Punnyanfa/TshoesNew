<template>

  <div>
    <!-- Header Component -->
    <Header />

    <!-- Main Content Section -->
    <div >
      <h1 class="text-center text-primary">Customize Your Shoes</h1>
      <p class="text-center">Use our 3D tool to design your perfect shoes.</p>

     
      <div class="text-center mt-4">
        <!-- Gán ref vào div thay vì dùng id -->
        <div ref="container" style="width: 50vw; height: 100vh;"></div>
      </div>
      <div class="mt-4">
        <button class="btn btn-success w-100">Save Design</button>
      </div>
    </div>

    <!-- Footer Component -->
    <Footer />
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import * as THREE from 'three'
import { GLTFLoader } from 'three/examples/jsm/loaders/GLTFLoader'
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls';
// Ref cho container chứa Three.js
const container = ref(null);

onMounted(() => {
  const scene = new THREE.Scene();
  const camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);
  camera.position.set(3, 3, 6);

  const renderer = new THREE.WebGLRenderer({ antialias: true });
  renderer.setSize(window.innerWidth, window.innerHeight);
  container.value.appendChild(renderer.domElement);

  const light = new THREE.AmbientLight(0xffffff, 2);
  scene.add(light);

  const directionalLight = new THREE.DirectionalLight(0xffffff, 2);
  directionalLight.position.set(5, 10, 5);
  scene.add(directionalLight);

  const controls = new OrbitControls(camera, renderer.domElement);
  controls.enableDamping = true;

  const loader = new GLTFLoader();
  loader.load('/Nikejordan4.glb', 
    (gltf) => {
      console.log("✅ Model loaded successfully:", gltf);
      scene.add(gltf.scene);
    },
    (xhr) => {
      console.log(`Loading progress: ${(xhr.loaded / xhr.total) * 100}%`);
    },
    (error) => {
      console.error('❌ Error loading model:', error);
    }
  );

  const animate = () => {
    requestAnimationFrame(animate);
    controls.update();
    renderer.render(scene, camera);
  };
  animate();

  window.addEventListener('resize', () => {
    camera.aspect = window.innerWidth / window.innerHeight;
    camera.updateProjectionMatrix();
    renderer.setSize(window.innerWidth, window.innerHeight);
  });
});
</script>

<style scoped>
.three-canvas {
  width: 100%;
  max-width: 600px;
  height: 400px;
  margin: 0 auto;
  background-color: #f0f0f0;
  border-radius: 10px;
}
</style>
