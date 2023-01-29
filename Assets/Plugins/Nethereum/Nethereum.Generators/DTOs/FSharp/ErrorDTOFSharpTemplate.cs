﻿using Nethereum.Generators.Core;
using Nethereum.Generators.CQS;

namespace Nethereum.Generators.DTOs
{
    public class ErrorDTOFSharpTemplate : ClassTemplateBase<ErrorDTOModel>
    {
        private ParameterABIFunctionDTOFSharpTemplate _parameterAbiErrorDtoFSharpTemplate;
        public ErrorDTOFSharpTemplate(ErrorDTOModel errorDTOModel) : base(errorDTOModel)
        {
            _parameterAbiErrorDtoFSharpTemplate = new ParameterABIFunctionDTOFSharpTemplate();
            ClassFileTemplate = new FSharpClassFileTemplate(Model, this);
        }

        public override string GenerateClass()
        {
            if (Model.HasParameters())
            {
                return
                    $@"{SpaceUtils.OneTab}[<Error(""{Model.ErrorABI.Name}"")>]
{SpaceUtils.OneTab}type {Model.GetTypeName()}() =
{SpaceUtils.TwoTabs}inherit ErrorDTO()
{_parameterAbiErrorDtoFSharpTemplate.GenerateAllProperties(Model.ErrorABI.InputParameters)}
{SpaceUtils.OneTab}";
            }
            else
            {
               return $@"{SpaceUtils.OneTab}[<Error(""{Model.ErrorABI.Name}"")>]
{SpaceUtils.OneTab}type {Model.GetTypeName()}() =
{SpaceUtils.TwoTabs}inherit ErrorDTO()
{SpaceUtils.OneTab}";
            }
        }
    }

}