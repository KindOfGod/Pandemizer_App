# Pandemizer Documentation

- [Basics](#basics)
  - [Pop](#pop)
  - [Iteration](#iteration)
  - [PopIndex](#popindex)
- [Characteristic Features](#characteristic-features)
  - [StateOfLive](#stateoflive)
  - [Age](#age)
  - [PreExistingCondition](#preexistingcondition)
  - [IsHospitalized](#ishospitalized)
- [Simulation Systems](#simulation-systems)
  - [Default Cycle](#default-cycle)
  - [Healthcare System](#healthcare-system)
- [Simulation Settings](#simulation-settings)
  - [General Settings](#general-settings)
  - [Virus Settings](#virus-settings)
  - [Health Distribution](#health-distribution)
  - [Age Distribution](#age-distribution)
  - [PreExistingCondition Distribution](#preexistingcondition-distribution)

---

## Basics

### Pop
> A Pop represents and encompasses a population group. This group is composed of various characteristic features of the group.

### Iteration
> An iteration represents one step in the simulation. This is equivalent to a period of one week in the real world.
> Each iteration contains a single [Pop Index](#popindex).

### PopIndex
> The PopIndex contains all the different [pops](#pop) that exist for the current [iteration](#iteration).

## Characteristic Features

---

### StateOfLive
> The StateOfLive describes the current state of health of a [pop](#pop). 
> This attribute separates into the following states:
> - Healthy: The pop is in good physical condition.
> - ImperceptiblyInfected: The pop is sick, but shows no symptoms. The disease is not noticeable.
> - Infected: The pop is sick. He shows noticeable symptoms.
> - HeavilyInfected: The pop is seriously ill. He shows very noticeable symptoms and is more vulnerable.
> - Dead: The Pop is dead. He is no longer simulated in the simulation.
> - Immune: The pop is immune. The pop is temporarily protected from the disease.

### Age
> The age of a [pop](#pop) has configurable influences on the pandemic course of a pop. 
> The initial [PopIndex](#popindex) is created at the beginning of the simulation with a specified age distribution.
> An important factor of age is the [EndangeredAgeGroup](#endangeredagegroup).
> The age of a pop is divided into four states:
> - Child
> - YoungAdult
> - Adult
> - Pensioner

### PreExistingCondition
> The PreExistingCondition indicates whether the [pop](#pop) has a pre-existing condition. 
> Such a condition generally has a negative impact on the course of the disease.

### IsHospitalized
> IsHospitalized shows whether a [pop](#pop) is hospitalised.

---

## Simulation Systems
>All basic [Characteristic Features](#characteristic-features) like IsEndangered or Age affect the specific calculations of the algorithm.
> The following systems describe the default cycles of the specific systems.

### Default Cycle
> - healthy [pops](#pop) can get infected
> - infected pops can get worse
> - infected pops can get immune
> - heavily infected pops can die
> - heavily infected pops can get immune
> - immune pops can get healthy

### Healthcare System
> - pops that are not hospitalized and infected can get into hospital after the [default cycle](#default-cycle)
> - Hospitalized [pops](#pop) heal or die faster than default cycle
> - Hospitalized pops which dont die continue with default cycle
> - Infected pops can get hospitalized after default cycle
> - Dead/Immune/Healthy pops can't be in hospital

---

## Simulation Settings

### General Settings
> The *Scope* indicates the number of people distributed among the [pops](#pop).
>
> The *IterationLimit* specifies the maximum number of [iterations](#iteration) the simulation runs through.
> 
> The *ProbabilityDeviation* indicates the deviations of the mathematically expected results of the simulation. 
> The lower the value, the more predictable the simulation.

### Virus Settings
> **General**
> 
> - The *Name* is the identifier of the virus.
> 
> - The *BaseInfectionRate* is the rate at which [pops](#pop) are infected by default.  
> The *EndangeredAgeInfectionRate* is the rate at which endangered pops are infected by default.
> 
> - The *EndangeredAgeGroup* indicates which [age](#age) group is particularly at risk from the disease.
> 
> - The *InfectionSpreadRate* is a multiplier that increases the rate of infections depending on the number of infected pops. 
> If all pops are infected, *BaseInfectionRate* times *InfectionSpreadRate * applies.
> 
> **Infection kind**
> 
> - The *InfectionSeverity* is the strength of the initial infection.
> 
> - The *RateOfGettingWorse* is rate at which the infection is getting worse.  
> The *EndangeredAgeRateOfGettingWorse* is rate at which the infection is getting worse for endangered pops.
> 
> **Rate of Death**
> 
> - The *BaseDeathRate* is the rate at which pops die.  
> The *EndangeredAgeDeathRate* is the rate at which endangered pops die.
> 
> **Rate of immunity**
>
> - The *BaseImmunityRate* is the rate at which pops get immune.  
> The *EndangeredImmunityRate* is the rate at which endangered pops get immune.
> - The *SurvivalInstinctMultiplier* multiplies the immunity rates if pop is heavily infected.
> 
> **Rate of losing immunity**
> 
> - The *ImmunityLostRate* is the rate at which pops lose immunity.
> 
> **Pre-existing condition modifier**
> 
> - The *PreConditionModifier* is the modifier for PreConditioned pops. They die or get immune faster.
> 
> **Hospital modifier**
> 
> - The *HospitalizedInfectedGetHealthy* modifier is the rate at which pops get out of hospital.
> 
> - The *HospitalizedHeavilyInfectedGetHealthy* modifier is the rate at which heavily infected pops get out of hospital.

### Health Distribution
> The *InitialPortionOfInfected* indicates how many [pops](#pop) of the different [age](#age) groups are infected. 
> This happens in a balanced way.
> 
> The *HealthIllnessSeverity* indicates what the [StateOfLive](#stateoflive) of the initially affected [pops](#pop) is.

### Age Distribution
> The *AgeProportionOfChildren* specifies the Children count.
> 
> The *AgeProportionOfYoungAdult* specifies the YoungAdult count.
> 
> The *AgeProportionOfAdult* specifies the Adult count.
> 
> The *AgeProportionOfPensioner* specifies the Pensioner count.

### PreExistingCondition Distribution
> The *InitialProportionOfPreConditioned* specifies the [PreConditioned](#preexistingcondition) count.
> The PreConditioned get distributed evenly between the [age](#age) groups.