import { ResolveFn } from '@angular/router';
import { UserFamiliesService } from '../_services/user-families.service';
import { inject } from '@angular/core';
import { Family } from '../_models/family';

//unused now maby later, if not - to be deleted
export const familyDetailedResolver: ResolveFn<Family> = (route, state) => {
  const familyService = inject(UserFamiliesService);

  return familyService.getFamily(Number(route.paramMap.get('familyId')));
};
