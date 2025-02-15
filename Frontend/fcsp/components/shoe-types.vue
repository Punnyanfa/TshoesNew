<template>
  <div class="min-vh-100 py-5 px-3">
    <!-- Header Section -->
    <div class="text-center mb-5">
      <h1 class="display-4 fw-bold text-dark">
        Understanding Your <span class="text-danger">Skin Type</span>
      </h1>
      <p class="text-muted">
        Discover your skin type and get personalized recommendations for the perfect skincare routine.
      </p>
    </div>

    <!-- Skin Types Grid -->
    <div class="row g-4">
      <div class="col-md-4" v-for="type in skinTypes" :key="type.title">
        <div class="card shadow-lg border-0">
          <img :src="type.image" :alt="type.title" class="card-img-top img-fluid">
          <div class="card-body">
            <h5 class="card-title">{{ type.title }}</h5>
            <p class="card-text">{{ type.description }}</p>
            
            <h6>Common Symptoms:</h6>
            <ul class="list-unstyled">
              <li v-for="symptom in type.symptoms" :key="symptom">
                <i class="bi bi-check-circle-fill text-danger me-2"></i>{{ symptom }}
              </li>
            </ul>

            <h6>Recommendations:</h6>
            <ul class="list-unstyled">
              <li v-for="rec in type.recommendations" :key="rec">
                <i class="bi bi-arrow-right-circle-fill text-primary me-2"></i>{{ rec }}
              </li>
            </ul>
            
            <button class="btn btn-danger w-100 mt-3">Learn More</button>
          </div>
        </div>
      </div>
    </div>

    <!-- Call to Action -->
    <div class="text-center mt-5">
      <div class="p-4 bg-light rounded">
        <h2 class="fw-bold">Not sure about your skin type?</h2>
        <p>Take our comprehensive skin analysis quiz to get detailed recommendations.</p>
        <router-link to="/quiz-landing" class="btn btn-primary">
          Take the Quiz
        </router-link>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      skinTypes: [
        {
          title: "Oily Skin",
          description: "High sebum production leading to shine and enlarged pores.",
          symptoms: ["Shiny appearance", "Enlarged pores", "Prone to acne", "Thick texture"],
          recommendations: ["Use oil-free products", "Try salicylic acid", "Regular exfoliation"],
          image: require('@/assets/oily_skin.jpg'),
        },
        {
          title: "Dry Skin",
          description: "Lacks moisture, causing tightness and flaking.",
          symptoms: ["Rough texture", "Flaking", "Tight feeling", "Fine lines"],
          recommendations: ["Rich moisturizers", "Gentle cleansers", "Hydrating serums"],
          image: require('@/assets/dry_skin.jpg'),
        },
        {
          title: "Combination Skin",
          description: "Oily T-zone with dry cheeks.",
          symptoms: ["Oily T-zone", "Dry cheeks", "Variable pores", "Breakouts"],
          recommendations: ["Zone-specific care", "Balanced products", "Gentle toning"],
          image: require('@/assets/combination_skin.jpg'),
        }
      ]
    };
  }
};
</script>

<style>
.card {
  transition: transform 0.3s ease-in-out;
}
.card:hover {
  transform: translateY(-5px);
}
</style>
