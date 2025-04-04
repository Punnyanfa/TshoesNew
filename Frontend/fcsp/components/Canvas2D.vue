<template>
    <div class="canvas-container">
      <v-stage
        ref="stage"
        :config="stageConfig"
        @dragstart="handleDragStart"
        @dragend="handleDragEnd"
        @transformend="handleTransformEnd"
        @click="handleStageClick"
      >
        <v-layer ref="layer">
          <v-image
            v-for="image in images"
            :key="image.id"
            :config="image"
            @transformend="handleTransformEnd"
            @click="handleImageClick"
          />
          <v-transformer ref="transformer" />
        </v-layer>
      </v-stage>
    </div>
  </template>
  
  <script setup>
  import { ref, onMounted, watch } from 'vue';
  import { VStage, VLayer, VImage, VTransformer } from 'vue3-konva';
  
  const props = defineProps({
    width: {
      type: Number,
      default: 800
    },
    height: {
      type: Number,
      default: 600
    }
  });
  
  const emit = defineEmits(['image-added', 'image-updated', 'image-load-error', 'image-selected']);
  
  const stage = ref(null);
  const layer = ref(null);
  const selectedImage = ref(null);
  const transformer = ref(null);
  
  const stageConfig = ref({
    width: props.width,
    height: props.height
  });
  
  const images = ref([]);
  
  // Xử lý khi kéo bắt đầu
  const handleDragStart = (e) => {
    const clickedOnEmpty = e.target === e.target.getStage();
    if (clickedOnEmpty) {
      selectedImage.value = null;
      if (transformer.value) {
        transformer.value.nodes([]);
      }
    }
  };
  
  // Xử lý khi kéo kết thúc
  const handleDragEnd = (e) => {
    if (selectedImage.value) {
      emit('image-updated', {
        id: selectedImage.value.id(),
        x: selectedImage.value.x(),
        y: selectedImage.value.y(),
        width: selectedImage.value.width() * selectedImage.value.scaleX(),
        height: selectedImage.value.height() * selectedImage.value.scaleY(),
        rotation: selectedImage.value.rotation(),
        scaleX: selectedImage.value.scaleX(),
        scaleY: selectedImage.value.scaleY()
      });
    }
  };
  
  // Xử lý khi transform kết thúc
  const handleTransformEnd = (e) => {
    const node = e.target;
    node.scaleX(Math.abs(node.scaleX()));
    node.scaleY(Math.abs(node.scaleY()));
    
    emit('image-updated', {
      id: node.id(),
      x: node.x(),
      y: node.y(),
      width: node.width() * node.scaleX(),
      height: node.height() * node.scaleY(),
      rotation: node.rotation(),
      scaleX: node.scaleX(),
      scaleY: node.scaleY()
    });
  };
  
  // Xử lý khi click vào stage
  const handleStageClick = (e) => {
    const clickedOnEmpty = e.target === e.target.getStage();
    if (clickedOnEmpty) {
      selectedImage.value = null;
      if (transformer.value) {
        transformer.value.nodes([]);
      }
    }
  };
  
  // Xử lý khi click vào image
  const handleImageClick = (e) => {
    const clickedOnTransformer = e.target.getParent().className === 'Transformer';
    if (clickedOnTransformer) {
      return;
    }
  
    const clickedOnImage = e.target.getType() === 'Image';
    if (clickedOnImage) {
      selectedImage.value = e.target;
      if (transformer.value) {
        transformer.value.nodes([e.target]);
      }
      // Emit sự kiện image-selected với thông tin chi tiết hơn
      emit('image-selected', {
        id: e.target.id(),
        x: e.target.x(),
        y: e.target.y(),
        width: e.target.width() * e.target.scaleX(),
        height: e.target.height() * e.target.scaleY(),
        rotation: e.target.rotation(),
        scaleX: e.target.scaleX(),
        scaleY: e.target.scaleY(),
        image: e.target.image()
      });
    } else {
      selectedImage.value = null;
      if (transformer.value) {
        transformer.value.nodes([]);
      }
    }
  };
  
  // Thêm ảnh mới vào canvas
  const addImage = (imageUrl) => {
    const image = new window.Image();
    image.src = imageUrl;
    
    image.onload = () => {
      // Tính toán kích thước phù hợp cho ảnh
      const maxWidth = stageConfig.value.width * 0.8;
      const maxHeight = stageConfig.value.height * 0.8;
      
      let width = image.width;
      let height = image.height;
      
      const aspectRatio = width / height;
      
      if (width > maxWidth) {
        width = maxWidth;
        height = width / aspectRatio;
      }
      if (height > maxHeight) {
        height = maxHeight;
        width = height * aspectRatio;
      }
      
      const konvaImage = {
        id: `image-${Date.now()}`,
        image: image,
        x: (stageConfig.value.width - width) / 2,
        y: (stageConfig.value.height - height) / 2,
        width: width,
        height: height,
        draggable: true,
        cornerRadius: 5,
        cornerStrokeWidth: 2,
        cornerStrokeColor: '#1a3c6c',
        cornerColor: '#ffffff',
        shadowColor: 'black',
        shadowBlur: 10,
        shadowOffset: { x: 5, y: 5 },
        shadowOpacity: 1
      };
      
      images.value.push(konvaImage);
      
      emit('image-added', {
        id: konvaImage.id,
        x: konvaImage.x,
        y: konvaImage.y,
        width: konvaImage.width,
        height: konvaImage.height,
        image: image
      });
    };
    
    image.onerror = (error) => {
      console.error('Lỗi khi load ảnh:', error);
      emit('image-load-error', error);
    };
  };
  
  // Xóa ảnh khỏi canvas
  const removeImage = (imageId) => {
    images.value = images.value.filter(img => img.id !== imageId);
    if (transformer.value) {
      transformer.value.nodes([]);
    }
  };
  
  // Reset canvas
  const resetCanvas = () => {
    images.value = [];
    if (transformer.value) {
      transformer.value.nodes([]);
    }
  };
  
  // Expose methods
  defineExpose({
    addImage,
    removeImage,
    resetCanvas
  });
  </script>
  
  <style scoped>
  .canvas-container {
    position: relative;
    background: #ffffff;
    border-radius: 8px;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
    overflow: hidden;
    opacity: 1;
  }
  
  :deep(.konvajs-content) {
    border-radius: 8px;
  }
  </style> 