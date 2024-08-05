import { ResolveFn } from '@angular/router';
import { UserFamiliesService } from '../_services/user-families.service';
import { inject } from '@angular/core';

export const familyDetailedResolver: ResolveFn<boolean> = (route, state) => {
  const familyService = inject(UserFamiliesService);

  return familyService.getFamily(route.paramMap.get('familyId')!);
};
