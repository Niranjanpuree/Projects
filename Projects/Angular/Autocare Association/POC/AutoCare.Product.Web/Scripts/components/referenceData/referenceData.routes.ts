
import {Routes} from '@angular/router';
import {ReferenceDataComponent} from './referenceData.component';
import { MakeRoutes} from '../make/make.routes';
import { ModelRoutes} from '../model/model.routes';
import { SubModelRoutes} from '../subModel/subModel.routes';
import { YearRoutes} from '../year/year.routes';
import { RegionRoutes} from '../region/region.routes';
import { VehicleTypeRoutes } from '../vehicleType/vehicleType.routes';
import { VehicleTypeGroupRoutes } from '../vehicleTypeGroup/vehicleTypeGroup.routes';
import { BrakeAbsRoutes } from '../brakeABS/brakeABS.routes';
import { BedLengthRoutes } from '../bedLength/bedLength.routes';
import { BedTypeRoutes } from '../bedType/bedType.routes';
import { BrakeSystemRoutes } from '../brakeSystem/brakeSystem.routes';
import { BrakeTypeRoutes } from '../brakeType/brakeType.routes';
import { BodyTypeRoutes } from '../bodyType/bodyType.routes';
import { BodyNumDoorRoutes } from '../bodyNumDoors/bodyNumDoors.routes';
import { EngineDesignationRoutes } from '../engineDesignation/engineDesignation.routes';
import { EngineVersionRoutes } from '../engineVersion/engineVersion.routes';
import { FuelDeliverySubTypeRoutes } from '../fuelDeliverySubType/fuelDeliverySubType.routes';
import { EngineVinRoutes } from '../engineVin/engineVin.routes';
import { FuelTypeRoutes } from '../fuelType/fuelType.routes';
import { AuthorizeService } from "../authorize.service";

export const ReferenceDataRoutes: Routes = [
    {
        path: 'referencedata', component: ReferenceDataComponent,
        children: [
            ...MakeRoutes,
            ...ModelRoutes,
            ...YearRoutes,
            ...SubModelRoutes,
            ...RegionRoutes,
            ...VehicleTypeRoutes,
            ...VehicleTypeGroupRoutes,
            ...BrakeAbsRoutes,
            ...BrakeSystemRoutes,
            ...BedLengthRoutes,
            ...BedTypeRoutes,
            ...BrakeTypeRoutes,
            ...BodyTypeRoutes,
            ...BodyNumDoorRoutes,
            ...EngineDesignationRoutes,
            ...EngineVersionRoutes,
            ...FuelDeliverySubTypeRoutes,
            ...EngineVinRoutes,
            ...FuelTypeRoutes,
            { path: '', redirectTo: 'make', pathMatch: 'full' },
        ],
        data: { activeTab: 'ReferenceData' },
        canActivate: [AuthorizeService]
    }
];