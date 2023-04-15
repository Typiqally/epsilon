<template>
  <div
    v-if="data"
    class="performance-dashboard"
  >
    <CompetenceProfileComponent
      :domain="data.hboIDomain"
      :data="data.professionalTaskOutcomes"
    />
    <CompetenceProfileLegend
      :domain="data.hboIDomain"
    />
    <CompetenceGraph
      :domain="data.hboIDomain"
      :data="data.decayingAveragesPerTask"
    />
    <PersonalDevelopmentMatrix
      :domain="data.hboIDomain"
      :data="data.decayingAveragesPerSkill"
    />
  </div>
  <RoundLoader v-else />
</template>

<script lang="ts" setup>
import {Api, HttpResponse, CompetenceProfile} from "../logic/Api";
import CompetenceProfileComponent from "@/components/CompetenceProfile.vue";
import CompetenceProfileLegend from "@/components/CompetenceProfileLegend.vue";
import CompetenceGraph from "@/components/CompetenceGraph.vue";
import PersonalDevelopmentMatrix from "@/components/PersonalDevelopmentGraph.vue";
import {onMounted, Ref, ref} from "vue";
import RoundLoader from "@/components/RoundLoader.vue";

const data: Ref<CompetenceProfile | undefined> = ref(undefined);
const App = new Api();

onMounted(() => {
    App.component.competenceProfileList()
        .then((r: HttpResponse<CompetenceProfile>) => {
            data.value = r.data
        })
})
</script>

<style scoped>
.performance-dashboard {
    grid-template-columns: 1fr;
}

@media screen and (min-width: 580px) {
    .performance-dashboard {
        display: grid;
        grid-template-columns: 5fr 1fr;
        gap: 4rem 0;
        align-items: center;
    }
}
</style>
