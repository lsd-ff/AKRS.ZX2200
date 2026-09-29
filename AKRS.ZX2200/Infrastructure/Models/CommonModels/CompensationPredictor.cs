using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using AKRS.Galaxy2.Infrastructure.Helper;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace AKRS.ZX2200.Infrastructure.Models.CommonModels;

/// <summary>
/// 补偿模型推理器
/// 部分模型冷启动加载较慢，尽量保持模型在内存中不要轻易释放
/// </summary>
public class CompensationPredictor : IDisposable
{
    private readonly InferenceSession session;
    private readonly Scaler scalerX;
    private readonly Scaler scalerY;
    private readonly bool usingScaler;
    private const string INPUT_NAME = "input";

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="modelPath">模型地址文件地址</param>
    /// <param name="scalerXPath">输入归一化器文件地址</param>
    /// <param name="scalerYPath">输出归一化器文件地址</param>
    public CompensationPredictor(
        string modelPath,
        string scalerXPath = null,
        string scalerYPath = null)
    {
        SessionOptions options = new()
        {
            GraphOptimizationLevel = GraphOptimizationLevel.ORT_ENABLE_ALL,
        };

        if (!File.Exists(modelPath))
        {
            throw new FileNotFoundException($"指定的模型文件{modelPath}不存在！");
        }

        this.session = new(modelPath, options);

        if (!string.IsNullOrEmpty(scalerXPath) && !string.IsNullOrEmpty(scalerYPath))
        {
            if (!File.Exists(scalerXPath))
            {
                throw new FileNotFoundException($"指定的归一化配置文件{scalerXPath}不存在！");
            }

            if (!File.Exists(scalerYPath))
            {
                throw new FileNotFoundException($"指定的归一化配置文件{scalerYPath}不存在！");
            }

            this.scalerX = Scaler.Load(scalerXPath);
            this.scalerY = Scaler.Load(scalerYPath);
            this.usingScaler = true;
        }
        else
        {
            this.usingScaler = false;
        }
    }

    /// <summary>
    /// 预测(所有参数及返回值单位均为um)
    /// Input-输入参数：
    /// Index-1 温漂Mark当前轴坐标系中的X坐标 - 温漂Mark拍摄位置X坐标
    /// Index-2 温漂Mark当前轴坐标系中的Y坐标 - 温漂Mark拍摄位置Y坐标
    /// Index-3 目标点轴坐标系中的X坐标
    /// Index-4 目标点轴坐标系中的Y坐标
    /// Output-输出结果：
    /// Index-1 目标点为对应X方向的补偿值
    /// Index-2 目标点为对应Y方向的补偿值
    /// </summary>
    /// <param name="input">输入</param>
    /// <returns>预测结果</returns>
    public float[] Predict(float[] input)
    {
        //DenseTensor<float> tensor = new(new[] { 1, input.Length });
        //float[] inputFeatures = this.usingScaler ? this.scalerX.Transform(input) : input;
        //for (int i = 0; i < input.Length; i++)
        //{
        //    tensor[0, i] = inputFeatures[i];
        //}

        //NamedOnnxValue[] inputs = new[] { NamedOnnxValue.CreateFromTensor(INPUT_NAME, tensor) };

        //using IDisposableReadOnlyCollection<DisposableNamedOnnxValue> results = this.session.Run(inputs);
        //float[] outputTensor = results.First().AsEnumerable<float>().ToArray();
        //float[] outputFeatures = this.usingScaler ? this.scalerY.InverseTransform(outputTensor) : outputTensor;
        //return outputFeatures;
        return null;
    }

    /// <summary>
    /// 释放资源
    /// </summary>
    public void Dispose()
    {
        this.session?.Dispose();
    }
}

/// <summary>
/// 归一化器
/// </summary>
public class Scaler
{
    /// <summary>
    /// 均值
    /// </summary>
    [JsonProperty("mean")] public float[] Mean { get; set; }

    /// <summary>
    /// 缩放因子
    /// </summary>
    [JsonProperty("scale")] public float[] Scale { get; set; }

    /// <summary>
    /// 归一化(正向变换)
    /// </summary>
    /// <param name="input">输入</param>
    /// <returns>归一化结果</returns>
    /// <exception cref="ArgumentException">配置文件异常</exception>
    public float[] Transform(float[] input)
    {
        if (input.Length != this.Mean.Length)
        {
            throw new ArgumentException("载入的配置文件有误");
        }

        float[] output = new float[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            output[i] = (input[i] - this.Mean[i]) / this.Scale[i];
        }

        return output;
    }

    /// <summary>
    /// 反归一化(逆向变换)
    /// </summary>
    /// <param name="input">输入</param>
    /// <returns>反归一化结果</returns>
    /// <exception cref="ArgumentException">配置文件异常</exception>
    public float[] InverseTransform(float[] input)
    {
        if (input.Length != this.Mean.Length)
        {
            throw new ArgumentException("载入的配置文件有误");
        }

        float[] output = new float[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            output[i] = input[i] * this.Scale[i] + this.Mean[i];
        }

        return output;
    }

    /// <summary>
    /// 载入归一化器配置文件
    /// json文件
    /// </summary>
    /// <param name="path">配置文件地址</param>
    /// <returns>归一化器实例</returns>
    public static Scaler Load(string path)
    {
        string json = File.ReadAllText(path);
        return JsonFormatHelper<Scaler>.DeserializeToObject(json);
    }
}